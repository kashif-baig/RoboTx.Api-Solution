using Messaging;
using System.IO.Ports;

namespace RoboTx.Api
{

    /// <summary>
    /// The main class through which input and output operations are performed with devices and components connected to the Arduino.
    /// </summary>
    public class RobotIO : IDisposable
    {
        readonly SerialPort _serialPort;
        readonly Queue<(int, DateTime)> _irCommandReceivedQueue;
        readonly MyMessageDeserializerFactory _factory;

        readonly DisplayLed _display7Seg;
        readonly DisplayLcd _displayLcd;
        readonly Trigger _trigger;
        readonly Analog _analog;
        readonly Digital _digital;

        readonly Motor _motor1;
        readonly Motor _motor2;
        readonly MotorConfig _motorConfig;

        readonly Sonar _sonar;

        readonly Servo _servo1;
        readonly Servo _servo2;
        readonly Servo _servo3;
        readonly Servo _servo4;
        readonly ServoConfig _servoConfig;
        readonly Config _config;

        readonly PulseCounter _pulseCounter;
        readonly ColourSensor _colour;
        readonly Switch _switch1;
        readonly Switch _switch2;
        readonly Switch _switch3;
        readonly Switch _switch4;
        readonly ConnectionState _connectionState;

        private volatile string _robotId = string.Empty;

        MessageListener _listener;
        MessageSender _msgSender;

        ConnectionKeeper _connectionKeeper;

        StreamWriter _serialStreamWriter;

        List<Task> _runningTasks = new List<Task>();
        Task _listenerTask;

        private bool disposedValue;
        private volatile bool _isClosing;

        /// <summary>
        /// Event is triggered when a Listener IO exception occurs. The event handler will likely be
        /// executed on a background thread.
        /// </summary>
        public event EventHandler<MessageListenerErrorEventArgs> ListenerErrorOccurred;

        /// <summary>
        /// Raises the ListenerErrorOccurred event when an IO error has been encountered by the MessageListener..
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnListenerErrorOccurred(MessageListenerErrorEventArgs e)
        {
            ListenerErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the user defined ROBOT_ID string set in the firmware file Settings.h. Use the value
        /// of this property to distinguish between different robots connected at
        /// the same time.
        /// </summary>
        public string RobotId { get => _robotId; internal set => _robotId = value; }

        /// <summary>
        /// Constructs an instance using a serial port name, 57600 baud rate and data terminal ready as false.
        /// </summary>
        /// <param name="port">Serial port name.</param>
        public RobotIO(string port) :this(port, 57600, false)
        {
            
        }

        /// <summary>
        /// Constructs an instance using a serial port name, baud rate and data terminal ready settings.
        /// </summary>
        /// <param name="port">Serial port name.</param>
        /// <param name="baud">Baud rate.</param>
        /// <param name="dtrEnable">Data terminal ready. Set to false when connecting directly to Arduino R3 Uno and Mega 2560,
        /// set to true for Leonardo and Arduino R4 Uno. Set to false when connecting via bluetooth.</param>
        public RobotIO(string port, int baud, bool dtrEnable)
        {
            _serialPort = new SerialPort(port, baud, Parity.None);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DtrEnable = dtrEnable;

            _display7Seg = new DisplayLed(this);
            _displayLcd = new DisplayLcd(this);

            _trigger = new Trigger(this);
            _analog = new Analog(this);

            _motor1 = new Motor(this, 1);
            _motor2 = new Motor(this, 2);
            _motorConfig = new MotorConfig();

            _servo1 = new Servo(this, 1);
            _servo2 = new Servo(this, 2);
            _servo3 = new Servo(this, 3);
            _servo4 = new Servo(this, 4);
            _servoConfig = new ServoConfig();

            _sonar = new Sonar(this);
            _config = new Config(this);
            _pulseCounter = new PulseCounter(this);
            _digital = new Digital(this);
            _colour = new ColourSensor(this);

            _switch1 = new Switch(this, 1);
            _switch2 = new Switch(this, 2);
            _switch3 = new Switch(this, 3);
            _switch4 = new Switch(this, 4);

            _irCommandReceivedQueue = new Queue<(int, DateTime)>();
            _factory = new MyMessageDeserializerFactory(this);

            // Start the message sender as a separate task.
            _msgSender = new MessageSender();
            _runningTasks.Add(Task.Run(() => _msgSender.ProcessMessageQueue()));

            _connectionKeeper = new ConnectionKeeper(this);
            _runningTasks.Add(Task.Run(() => _connectionKeeper.KeepAliveTask()));

            _connectionState = new ConnectionState(this);
            IsClosing = false;
        }

        /// <summary>
        /// Initiates serial connection with Arduino using port and baud rates specified in constructor.
        /// </summary>
        /// <exception cref="FileNotFoundException">Serial port was not found.</exception>
        public void Connect()
        {
            IsClosing = false;

            _serialPort.Open();
            _serialStreamWriter = new StreamWriter(_serialPort.BaseStream);

            string osVersion = Environment.OSVersion.Platform.ToString().ToLower();

            if (osVersion != "win32nt")
            {
                // On some OS platforms, opening the serial port resets the Arduino.
                // The additional delay allows the Arduino to settle down.
                Thread.Sleep(600);
            }

            ConnectionMessage connMsg = new ConnectionMessage(_serialStreamWriter);
            connMsg.Open();
            connMsg.Serialize();

            _listener = new MessageListener(_serialPort.BaseStream, _factory);
            _listener.IOErrorOccurred += listener_IOErrorOccurred;

            // Start the message listener as a separate task.
            _listenerTask = _listener.ProcessMessageStream();

            // A delay to allow incoming analog or digital input values to be received.
            Thread.Sleep(700);
        }

        private void listener_IOErrorOccurred(object sender, MessageListenerErrorEventArgs e)
        {
            OnListenerErrorOccurred(e);
        }

        /// <summary>
        /// Returns true if the serial port is connected to the Arduino, false otherwise.
        /// </summary>
        internal bool IsConnected => _serialPort != null && _serialPort.IsOpen;

        /// <summary>
        /// Returns true if the method <see cref="NotifyClosing"/> was previously called.
        /// Returns false otherwise. This property is for monitoring by background threads
        /// which should exit gracefully when the property returns true.
        /// </summary>
        internal bool IsClosing { get => _isClosing; private set => _isClosing = value; }

        /// <summary>
        /// Informs the connected state of the computer with the Arduino.
        /// </summary>
        public ConnectionState ConnectionState => _connectionState;

        /// <summary>
        /// The method should be called in the main application to signal to background threads monitoring property <see cref="IsClosing">IsClosing</see> that they should terminate gracefully.
        /// </summary>
        public void NotifyClosing()
        {
            IsClosing = true;
        }

        /// <summary>
        /// Writes a string to the 7 segment LED display.
        /// </summary>
        public DisplayLed LedDisplay => _display7Seg;

        /// <summary>
        /// Sends text output to the LCD display.
        /// </summary>
        public DisplayLcd Display => _displayLcd;


        /// <summary>
        /// A digital trigger for generating repeating pulse cycle patterns.
        /// </summary>
        public Trigger Trigger => _trigger;

        /// <summary>
        /// Allows for configuring of analog input, and exposing analog readings.
        /// </summary>
        public Analog Analog => _analog;

        /// <summary>
        /// Configuration options for DC motors.
        /// </summary>
        public MotorConfig MotorConfig { get => _motorConfig; }

        /// <summary>
        /// Controls speed and direction of motor 1.
        /// </summary>
        public Motor Motor1 => _motor1;

        /// <summary>
        /// Controls speed and direction of motor 2.
        /// </summary>
        public Motor Motor2 => _motor2;

        /// <summary>
        /// Configuration options for servo motors.
        /// </summary>
        public ServoConfig ServoConfig => _servoConfig;

        /// <summary>
        /// Sets the range and position of servo motor 1.
        /// </summary>
        public Servo Servo1 => _servo1;

        /// <summary>
        /// Sets the range and position of servo motor 2.
        /// </summary>
        public Servo Servo2 => _servo2;

        /// <summary>
        /// Sets the range and position of servo motor 3.
        /// </summary>
        public Servo Servo3 => _servo3;

        /// <summary>
        /// Sets the range and position of servo motor 4.
        /// </summary>
        public Servo Servo4 => _servo4;

        /// <summary>
        /// Uses sonar to calculate distance by sending a ping and measuring the time lapsed before receiving the echo.
        /// </summary>
        public Sonar Sonar => _sonar;

        /// <summary>
        /// Sets a digital output to on or off.
        /// </summary>
        public Switch Switch1 => _switch1;
        /// <summary>
        /// Sets a digital output to on or off.
        /// </summary>
        public Switch Switch2 => _switch2;
        /// <summary>
        /// Sets a digital output to on or off.
        /// </summary>
        public Switch Switch3 => _switch3;
        /// <summary>
        /// Sets a digital output to on or off.
        /// </summary>
        public Switch Switch4 => _switch4;

        /// <summary>
        /// Reports values read using TCS34725 sensor.
        /// </summary>
        public ColourSensor ColourSensor => _colour;

        /// <summary>
        /// Configures some devices connected to the robot.
        /// </summary>
        internal Config Config => _config;

        /// <summary>
        /// Calculates period of input pulses on a designated Arduino pin.
        /// </summary>
        public PulseCounter PulseCounter => _pulseCounter;

        /// <summary>
        /// Exposes digital input readings.
        /// </summary>
        public Digital Digital => _digital;


        private volatile int _prevIRCommand = -1;
        private DateTime _lastIRCommandTime = DateTime.MinValue;

        /// <summary>
        /// Gets the value and pressed state of an IR command button if one has been received by pressing a button on an IR remote control.
        /// </summary>
        /// <returns>A tuple consisting of the IR command code and its pressed state. A value of -1 for the IR command
        /// indicates none received. A value of true for the pressed state indicates button in pressed state.
        /// False indicates button released.</returns>
        public IrCommand GetIRCommand()
        {
            lock (_irCommandReceivedQueue)
            {
                if (_irCommandReceivedQueue.Count != 0)
                {
                    (int peekCmd, DateTime cmdTime) = _irCommandReceivedQueue.Peek();

                    if (_prevIRCommand == -1)
                    {
                        _irCommandReceivedQueue.Dequeue();
                        _lastIRCommandTime = cmdTime;
                        _prevIRCommand = peekCmd;
                        return new IrCommand { Code = peekCmd, ButtonPressed = true }; //(peekCmd, true);
                    }
                    if (peekCmd != _prevIRCommand)
                    {
                        int tmpCmd = _prevIRCommand;
                        _prevIRCommand = -1;
                        return new IrCommand { Code = tmpCmd, ButtonPressed = false, ButtonReleased = true };//(tmpCmd, false);
                    }
                    if (cmdTime.Subtract(_lastIRCommandTime).TotalSeconds < .15)
                    {
                        _irCommandReceivedQueue.Dequeue();
                        _lastIRCommandTime = cmdTime;
                        return new IrCommand { Code = -1, ButtonPressed = false };//(-1, false);
                    }
                    else
                    {
                        _prevIRCommand = -1;
                    }
                }
                else if (_prevIRCommand != -1 && DateTime.Now.Subtract(_lastIRCommandTime).TotalSeconds > .15)
                {
                    int tmpCmd = _prevIRCommand;
                    _prevIRCommand = -1;
                    return new IrCommand { Code = tmpCmd, ButtonPressed = false, ButtonReleased = true };//(tmpCmd, false);
                }
                return new IrCommand { Code = -1, ButtonPressed = false }; //(-1, false);
            }
        }

        internal void QueueIrCommand(int irCommand)
        {
            lock (_irCommandReceivedQueue)
            {
                if (_irCommandReceivedQueue.Count <= 3)
                {
                    _irCommandReceivedQueue.Enqueue((irCommand, DateTime.Now));
                }
            }
        }

        internal MessageSender MessageSender => _msgSender;

        internal StreamWriter StreamWriter => _serialStreamWriter;

        private bool InBreakState => _serialPort != null ? _serialPort.BreakState : false;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        internal void CheckSerialState()
        {
            if (InBreakState)
            {
                throw new IOException(ErrorMessageText.SERIAL_PORT_ERROR_STATE);
            }
            if (StreamWriter == null)
            {
                throw new IOException(ErrorMessageText.SERIAL_PORT_NOT_OPEN);
            }
        }


        /// <summary>
        /// Closes connection with Arduino. DC motors and servo motors will be disabled, the digital trigger
        /// and digital switches will turn off, and the LED display will clear. 
        /// </summary>
        public void Close()
        {
            _msgSender?.Cancel();
            _listener?.Cancel();

            _connectionKeeper?.Cancel();

            if (_runningTasks.Count > 0)
            {
                Task.WaitAll(_runningTasks.ToArray());
            }
            _runningTasks.Clear();

            if (_serialPort.IsOpen)
            {
                ConnectionMessage connMsg = new ConnectionMessage(StreamWriter);
                connMsg.Close();
                connMsg.Serialize();
                Thread.Sleep(10);
            }

            _serialPort.Close();

            _listenerTask?.Wait();
            _listenerTask = null;

            if (_listener != null)
            {
                _listener.IOErrorOccurred -= listener_IOErrorOccurred;
            }
        }

        /// <summary>
        /// Closes connection with the Arduino and releases resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    Close();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Closes connection with the Arduino and releases resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
