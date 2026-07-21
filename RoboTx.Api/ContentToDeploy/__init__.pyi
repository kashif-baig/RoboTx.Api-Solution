import typing, clr, abc
from System import Array_1, MulticastDelegate, IAsyncResult, AsyncCallback, IDisposable
from System.Reflection import MethodInfo

class AccelValues:
    """Holds the most recent accelerometer values for the X, Y and Z axis."""
    def __init__(self) -> None: ...
    @property
    def X(self) -> float: 
        """Gets accelerometer X axis value."""
        ...
    @property
    def Y(self) -> float: 
        """Gets accelerometer Y axis value."""
        ...
    @property
    def Z(self) -> float: 
        """Gets accelerometer Z axis value."""
        ...
    def Deconstruct(self, x: clr.Reference[float], y: clr.Reference[float], z: clr.Reference[float]) -> None: 
        """Unpacks X,Y and Z axis of accelerometer tuple."""
        ...
    def UseConverter(self, converter: MPUConverter) -> None: 
        """Registers a function to convert raw accelerometer sensor values to either calibrated values and or a different scale."""
        ...


class Analog(ContinuousSensor):
    """Configures and reports analog inputs via Arduino pins A0 to A7."""
    @property
    def A0(self) -> AnalogInput: 
        """Gets the analog reading of pin A0 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A1(self) -> AnalogInput: 
        """Gets the analog reading of pin A1 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A2(self) -> AnalogInput: 
        """Gets the analog reading of pin A2 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A3(self) -> AnalogInput: 
        """Gets the analog reading of pin A3 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A4(self) -> AnalogInput: 
        """Gets the analog reading of pin A4 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A5(self) -> AnalogInput: 
        """Gets the analog reading of pin A5 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A6(self) -> AnalogInput: 
        """Gets the analog reading of pin A6 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def A7(self) -> AnalogInput: 
        """Gets the analog reading of pin A7 whose value is between 0 and 1023 inclusive. The pin must be enabled for analog input before values become available."""
        ...
    @property
    def IsEnabled(self) -> bool: 
        """Returns true if the sensor has been enabled."""
        ...
    @property
    def IsReady(self) -> bool: 
        """Returns true if the sensor values are available for reading."""
        ...
    def DisableInputsA(self, inputPins: Array_1[int]) -> None: 
        """Disables analog inputs whose sources are Arduino pins A0, A1, A2, A3, A4, A5, A6 and A7. Digital inputs for the specified Arduino pins are also disabled."""
        ...
    def EnableInputsA(self, inputPins: Array_1[int]) -> None: 
        """Enables analog inputs whose sources are Arduino pins A0, A1, A2, A3, A4, A5, A6 and A7. The default sample rate is 10Hz. A short delay may need to be implemented after a call to this method to allow analog values to start being received. Enable only the input sources that are needed. Digital inputs for the specified Arduino pins are disabled."""
        ...
    def SetSampleRate(self, sampleRateHz: int) -> None: 
        """Sets the rate at which the analog pins are sampled."""
        ...
    def UseConverter(self, converter: AnalogConverter, input: Array_1[AnalogInput]) -> None: 
        """Assigns a function to one or more analog inputs to convert the normal range of analog values from 0 to 1023, to another range of values."""
        ...


class AnalogConverter(MulticastDelegate):
    """Analog input converter"""
    def __init__(self, object: typing.Any, method: int) -> None: ...
    @property
    def Method(self) -> MethodInfo: ...
    @property
    def Target(self) -> typing.Any: ...
    def BeginInvoke(self, inputValue: float, callback: AsyncCallback, object: typing.Any) -> IAsyncResult: ...
    def EndInvoke(self, result: IAsyncResult) -> float: ...
    def Invoke(self, inputValue: float) -> float: ...


class AnalogInput:
    """Represents the value of a given analog input."""
    @property
    def Value(self) -> float: 
        """Gets the analog input value."""
        ...
    @Value.setter
    def Value(self, value: float) -> float: ...
    # Operator not supported op_Implicit(a: AnalogInput)
    def ToString(self) -> str: 
        """Returns the string representation of the value."""
        ...


class ColourSensor(ContinuousSensor):
    """Reports RGB and HSL values detected using TCS34725 I2C sensor. HSL values may give better results when detecting colours. For each colour to detect, obtain reference values with the sensor close to the object, and with the sensor further away. Compare readings against the range of reference values to identify the colour. This feature is only available in the Education Edition of the Robo-Tx API."""
    @property
    def IsEnabled(self) -> bool: 
        """Returns true if the sensor has been enabled."""
        ...
    @property
    def IsReady(self) -> bool: 
        """Returns true if the sensor values are available for reading."""
        ...
    def Disable(self) -> None: 
        """Stops the colour sensor reporting colour values."""
        ...
    def GetHSL(self) -> HSLColour: 
        """Gets the colour as H, S and L values reported by the colour sensor once it has been enabled. How long it takes for colour values to start being reported, and the subsequent reporting interval will depend in the integration time specified in the"""
        ...
    def GetRGBC(self) -> RGBColour: 
        """Gets the red, green, blue and clear values reported by the colour sensor once it has been enabled. How long it takes for colour values to start being reported, and the subsequent reporting interval will depend in the integration time specified in the"""
        ...
    # Skipped Enable due to it being static, abstract and generic.

    Enable : Enable_MethodGroup
    class Enable_MethodGroup:
        @typing.overload
        def __call__(self) -> None:...
        @typing.overload
        def __call__(self, integrationTime: int, gain: int) -> None:...



class Config:
    """Configures the type of motor driver connected to the Arduino."""
    pass


class ConnectionState:
    """Informs the connected state of the computer with the Arduino."""
    @property
    def IsClosing(self) -> bool: 
        """Returns true if the method NotifyClosing() was previously called. Returns false otherwise. This property is for monitoring by background threads which should exit gracefully when the property returns true."""
        ...
    @property
    def IsConnected(self) -> bool: 
        """Returns true if the serial port is connected to the Arduino, false otherwise."""
        ...


class ContinuousSensor(abc.ABC):
    """Base class for any sensor that continuously reports values, after being enabled. Once the values are available, the state of the sensor is set to ready."""
    @property
    def IsEnabled(self) -> bool: ...
    @property
    def IsReady(self) -> bool: ...
    def Disable(self) -> None: 
        """Use to disable the sensor and stop reporting values."""
        ...
    def Enable(self) -> None: 
        """Use to enable the sensor and start reporting values."""
        ...


class DHTSensor(ContinuousSensor):
    """Reports temperature and relative hudity values using a DHT20 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
    @property
    def Humidity(self) -> float: 
        """Gets current relative humidity reading as a percentage."""
        ...
    @Humidity.setter
    def Humidity(self, value: float) -> float: ...
    @property
    def IsEnabled(self) -> bool: 
        """Returns true if the sensor has been enabled."""
        ...
    @property
    def IsReady(self) -> bool: 
        """Returns true if the sensor values are available for reading."""
        ...
    @property
    def Temperature(self) -> float: 
        """Gets current temperature reading in degrees celsius."""
        ...
    @Temperature.setter
    def Temperature(self, value: float) -> float: ...
    def Disable(self) -> None: 
        """Stops the DHT sensor reporting values."""
        ...
    def Enable(self) -> None: 
        """Enables the DHT sensor to report temperature and humidity values."""
        ...


class Digital:
    """Configures and reports digital inputs via Arduino pins defined in the firmware profile. By default the input pins are A0 to A4, but can be overridden for a specific profile using firmware macro DIGITAL_INPUT_PINS. Digital inputs are identified by their index position. I.e. index 0 by default corresponds to A0, index 1 to A1 etc. Also reports received IR commands sent by an IR remote control."""
    @property
    def IN0(self) -> DigitalInput: 
        """Gets the digital reading for input 0 whose value is either true or false. The corresponding pin must be enabled for digital input before values become available."""
        ...
    @property
    def IN1(self) -> DigitalInput: 
        """Gets the digital reading for input 1 whose value is either true or false. The corresponding pin must be enabled for digital input before values become available."""
        ...
    @property
    def IN2(self) -> DigitalInput: 
        """Gets the digital reading for input 2 whose value is either true or false. The corresponding pin must be enabled for digital input before values become available."""
        ...
    @property
    def IN3(self) -> DigitalInput: 
        """Gets the digital reading for input 3 whose value is either true or false. The corresponding pin must be enabled for digital input before values become available."""
        ...
    @property
    def IN4(self) -> DigitalInput: 
        """Gets the digital reading for input 4 whose value is either true or false. The corresponding pin must be enabled for digital input before values become available."""
        ...
    @property
    def InputCount(self) -> int: 
        """Returns the number of digital inputs that are set to true."""
        ...
    def ClearInputEvents(self, timeoutMs: int = ...) -> None: 
        """Clears queued inputs from the internal buffer and resets the current readings to false. Clears for the duration specified by parameter timeoutMs."""
        ...
    def DisableInputs(self, inputPins: Array_1[int]) -> None: 
        """Disables digital inputs and events whose sources are Arduino pins defined in the firmware profile. Analog inputs for corresponding Arduino pins are disabled. The inverted configuration of any disabled inputs are reset. Input pins are identified by their index position in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS."""
        ...
    def EnableInputs(self, inputPins: Array_1[int]) -> None: 
        """Enables digital inputs and events whose sources are Arduino pins defined in the firmware profile. A short delay may need to be implemented after a call to this method to allow digital values to start being received. Enable only the input sources that are needed. Analog inputs for corresponding Arduino pins are disabled. Input pins are identified by their index position in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS."""
        ...
    def GetInputEvent(self) -> int: 
        """If an input event (e.g. button press) has been detected, get its value or return -1 otherwise. Use class"""
        ...
    def GetIRCommand(self) -> IrCommand: 
        """Gets the value and pressed state of an IR command button if one has been received by pressing a button on an IR remote control."""
        ...
    def InvertInputs(self, inputPins: Array_1[int]) -> None: 
        """Configures Arduino digital inputs to be detected on Low (inverted) signal. This method should be called before enabling any pins for digital input. Input pins are identified by their index position in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS."""
        ...
    def ResetInputsState(self, inputPins: Array_1[int]) -> None: 
        """Resets the state of the specified inputs from true to false. Input pins are identified by their index position in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS."""
        ...
    def ToString(self) -> str: 
        """Returns the digital inputs as a binary string."""
        ...
    def UseIrCommandConverter(self, converter: IrCommandConverter) -> None: 
        """Registers a function to convert IR command codes to a string value."""
        ...


class DigitalInput:
    """Represents the value of a given digital input."""
    @property
    def Value(self) -> bool: 
        """Gets the digital input value."""
        ...
    # Operator not supported op_Implicit(d: DigitalInput)
    def ToString(self) -> str: 
        """Returns the string representation of the value."""
        ...


class DisplayLcd:
    """Writes text to a 16x2 or 16x4 I2C LCD display."""
    def Clear(self) -> None: 
        """Clears the display."""
        ...
    def PrintAt(self, col: int, row: int, text: str) -> None: 
        """Displays text starting at a specific location of the LCD display. Causes the LCD backlight to switch on. The backlight may switch off after a predefined duration in the firmware."""
        ...
    def Sleep(self) -> None: 
        """Switches off the LCD backlight."""
        ...
    def WakeUp(self) -> None: 
        """Switches on the LCD backlight. The backlight may switch off after a predefined duration in the firmware."""
        ...


class DisplayLed:
    """Writes a string to a 4 digit 7 segment LED display. The Arduino pins assigned for the display are configured in the firmware settings."""
    # Skipped Write due to it being static, abstract and generic.

    Write : Write_MethodGroup
    """Writes a value to the 7 segment display."""
    class Write_MethodGroup:
        @typing.overload
        def __call__(self, value: float) -> None:...
        # Method Write(value : Single) was skipped since it collides with above method
        # Method Write(value : Int32) was skipped since it collides with above method
        @typing.overload
        def __call__(self, text: str) -> None:...



class GyroValues:
    """Holds the most recent gyrometer values for the X, Y and Z axis."""
    def __init__(self) -> None: ...
    @property
    def X(self) -> float: 
        """Gets gyrometer X axis value."""
        ...
    @property
    def Y(self) -> float: 
        """Gets gyrometer Y axis value."""
        ...
    @property
    def Z(self) -> float: 
        """Gets gyrometer Z axis value."""
        ...
    def Deconstruct(self, x: clr.Reference[float], y: clr.Reference[float], z: clr.Reference[float]) -> None: 
        """Unpacks X,Y and Z axis of gyrometer tuple."""
        ...
    def UseConverter(self, converter: MPUConverter) -> None: 
        """Registers a function to convert raw gyrometer sensor values to either calibrated values and or a different scale."""
        ...


class HSLColour:
    """Represents colour as Hue, Saturation and Lightness values."""
    @property
    def Hue(self) -> float: 
        """Hue component."""
        ...
    @Hue.setter
    def Hue(self, value: float) -> float: ...
    @property
    def Lightness(self) -> float: 
        """Lightness component."""
        ...
    @Lightness.setter
    def Lightness(self, value: float) -> float: ...
    @property
    def Saturation(self) -> float: 
        """Saturation component."""
        ...
    @Saturation.setter
    def Saturation(self, value: float) -> float: ...
    def Deconstruct(self, hue: clr.Reference[float], saturation: clr.Reference[float], lightness: clr.Reference[float]) -> None: 
        """Unpacks HSL Colour tuple."""
        ...


class Input(abc.ABC):
    """Input constants representing the pressing, holding and releasing of buttons, or triggering of contacts on Arduino pins defined in the firmware."""
    BUTTON_1_PRESSED : int
    """Button 1 was pressed."""
    BUTTON_1_RELEASED : int
    """Button 1 was released after a short press."""
    BUTTON_1_SUSTAIN_RELEASED : int
    """Button 1 was released after a long press."""
    BUTTON_1_SUSTAINED : int
    """Button 1 is held down."""
    BUTTON_2_PRESSED : int
    """Button 2 was pressed."""
    BUTTON_2_RELEASED : int
    """Button 2 was released after a short press."""
    BUTTON_2_SUSTAIN_RELEASED : int
    """Button 2 was released after a long press."""
    BUTTON_2_SUSTAINED : int
    """Button 2 is held down."""
    BUTTON_3_PRESSED : int
    """Button 3 was pressed."""
    BUTTON_3_RELEASED : int
    """Button 3 was released after a short press."""
    BUTTON_3_SUSTAIN_RELEASED : int
    """Button 3 was released after a long press."""
    BUTTON_3_SUSTAINED : int
    """Button 3 is held down."""
    IN0_TRIGGERED : int
    """Digital signal was detected on IN0."""
    IN1_TRIGGERED : int
    """Digital signal was detected on IN1."""
    IN2_TRIGGERED : int
    """Digital signal was detected on IN2."""
    IN3_TRIGGERED : int
    """Digital signal was detected on IN3."""
    IN4_TRIGGERED : int
    """Digital signal was detected on IN4."""
    None : int


class IrCommand:
    """Represents IR Command received by the IR remote sensor."""
    @property
    def ButtonPressed(self) -> bool: 
        """Gets the pressed state of the command button. True indicates pressed."""
        ...
    @ButtonPressed.setter
    def ButtonPressed(self, value: bool) -> bool: ...
    @property
    def ButtonReleased(self) -> bool: 
        """Gets the released state of the command button. True indicates released."""
        ...
    @ButtonReleased.setter
    def ButtonReleased(self, value: bool) -> bool: ...
    @property
    def Code(self) -> int: 
        """Gets the IR command code."""
        ...
    @Code.setter
    def Code(self, value: int) -> int: ...
    @property
    def Name(self) -> str: 
        """Uses the registered IrCommandConverter function to convert a received IR command code to a string value."""
        ...
    @property
    def Received(self) -> bool: 
        """Returns true if an IR command has been received."""
        ...
    # Skipped Deconstruct due to it being static, abstract and generic.

    Deconstruct : Deconstruct_MethodGroup
    class Deconstruct_MethodGroup:
        @typing.overload
        def __call__(self, code: clr.Reference[int], buttonPressed: clr.Reference[bool], buttonReleased: clr.Reference[bool]) -> None:...
        @typing.overload
        def __call__(self, code: clr.Reference[int], name: clr.Reference[str], buttonPressed: clr.Reference[bool], buttonReleased: clr.Reference[bool]) -> None:...



class IrCommandConverter(MulticastDelegate):
    """Infra red remote sensor command converter. Use for converting command codes to a string value."""
    def __init__(self, object: typing.Any, method: int) -> None: ...
    @property
    def Method(self) -> MethodInfo: ...
    @property
    def Target(self) -> typing.Any: ...
    def BeginInvoke(self, irCode: int, callback: AsyncCallback, object: typing.Any) -> IAsyncResult: ...
    def EndInvoke(self, result: IAsyncResult) -> str: ...
    def Invoke(self, irCode: int) -> str: ...


class LightMeter(ContinuousSensor):
    """Reports LUX values from a BH1750 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
    @property
    def IsEnabled(self) -> bool: 
        """Returns true if the sensor has been enabled."""
        ...
    @property
    def IsReady(self) -> bool: 
        """Returns true if the sensor values are available for reading."""
        ...
    @property
    def LuxValue(self) -> int: 
        """Gets the LUX value reported by the light meter sensor."""
        ...
    @LuxValue.setter
    def LuxValue(self, value: int) -> int: ...
    def Disable(self) -> None: 
        """Stops the light meter sensor reporting LUX values."""
        ...
    def Enable(self) -> None: 
        """Enables the light meter sensor to report LUX values."""
        ...
    # Operator not supported op_Implicit(a: LightMeter)


class Motor:
    """Controls speed, acceleration, duration and direction of a motor. The Arduino pins assigned for motors are configured in the firmware settings."""
    @property
    def DurationLapsed(self) -> bool: 
        """Returns true if a previously set duration for driving the motor has lapsed. Returns false otherwise."""
        ...
    def SetAcceleration(self, timeToMaxSpeed: float) -> None: 
        """Configures motor acceleration by specifying the time (in seconds) it takes to reach maximum speed from stationary position."""
        ...
    def StopAccelerating(self) -> None: 
        """If motor is accelerating, holds the motor at the current speed."""
        ...
    # Skipped Drive due to it being static, abstract and generic.

    Drive : Drive_MethodGroup
    """Drives the motor at a percentage of its maximum speed (either forward or reverse)."""
    class Drive_MethodGroup:
        def __call__(self, speedPercent: float) -> None:...
        # Method Drive(speedPercent : Int32) was skipped since it collides with above method

    # Skipped DriveForDuration due to it being static, abstract and generic.

    DriveForDuration : DriveForDuration_MethodGroup
    """Drives the motor at a percentage of its maximum speed (either forward or reverse) for a specified duration."""
    class DriveForDuration_MethodGroup:
        def __call__(self, speedPercent: float, duration: float) -> None:...
        # Method DriveForDuration(speedPercent : Int32, duration : Single) was skipped since it collides with above method

    # Skipped DriveNoAccel due to it being static, abstract and generic.

    DriveNoAccel : DriveNoAccel_MethodGroup
    """Drives the motor at a percentage of its maximum speed (either forward or reverse) and overrides any previously set acceleration."""
    class DriveNoAccel_MethodGroup:
        def __call__(self, speedPercent: float) -> None:...
        # Method DriveNoAccel(speedPercent : Int32) was skipped since it collides with above method



class MotorConfig:
    """Configuration options for DC motors."""
    def SetSpeedLimits(self, maxReverseSpeed: int, maxForwardSpeed: int, motor: Motor) -> None: 
        """Sets the reverse and forward speed limits of a motor."""
        ...
    def SetSpeedMultiplier(self, speedMultiplier: float, motor: Motor) -> None: 
        """Sets speed multiplier for motor. A negative value will reverse the motor direction. Allows for fine tuning of the motor speed to match the speed of the other motor."""
        ...


class MPUConverter(MulticastDelegate):
    """MPU sensor converter. Use to convert raw MPU sensor readings to either calibrated values and or a different scale."""
    def __init__(self, object: typing.Any, method: int) -> None: ...
    @property
    def Method(self) -> MethodInfo: ...
    @property
    def Target(self) -> typing.Any: ...
    def BeginInvoke(self, axis: str, rawValue: int, callback: AsyncCallback, object: typing.Any) -> IAsyncResult: ...
    def EndInvoke(self, result: IAsyncResult) -> float: ...
    def Invoke(self, axis: str, rawValue: int) -> float: ...


class MPUSensor(ContinuousSensor):
    """Report accelerometer and gyro readings from an MPU6050 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
    @property
    def Accel(self) -> AccelValues: 
        """Reports accelerometer readings from an MPU6050 sensor."""
        ...
    @property
    def Gyro(self) -> GyroValues: 
        """Reports gyrometer readings from an MPU6050 sensor."""
        ...
    @property
    def IsEnabled(self) -> bool: 
        """Returns true if the sensor has been enabled."""
        ...
    @property
    def IsReady(self) -> bool: 
        """Returns true if the sensor values are available for reading."""
        ...
    def Disable(self) -> None: 
        """Stops the MPU6050 sensor from reporting readings."""
        ...
    def Enable(self) -> None: 
        """Enables the MPU6050 sensor to begin reporting readings."""
        ...


class PulseCounter:
    """Configures and calculates period of input pulses on configurable Arduino pin, as defined in the RoboTx firmware settings (default is A2). This feature is only available in the Education Edition of the Robo-Tx API."""
    @property
    def Period(self) -> int: 
        """Gets the pulse period (in milliseconds) reported by the pulse counter. Pulse counter must be enabled before values become available."""
        ...
    @Period.setter
    def Period(self, value: int) -> int: ...    
    def Disable(self) -> None: 
        """Disables the pulse counter."""
        ...
    def Enable(self, timeoutMs: int, triggerEdge: int) -> None: 
        """Initializes and enables the pulse counter. Used for measuring pulses applied to a designated input pin defined. Max pulse frequency 500hz. Inputs on A2 will not be registered as events whilst pulse counting is enabled."""
        ...


class RGBColour:
    """Represents colour as Red, Green, Blue and Clear values."""
    @property
    def Blue(self) -> int: 
        """Blue component."""
        ...
    @Blue.setter
    def Blue(self, value: int) -> int: ...
    @property
    def Clear(self) -> int: 
        """Clear component."""
        ...
    @Clear.setter
    def Clear(self, value: int) -> int: ...
    @property
    def Green(self) -> int: 
        """Green component."""
        ...
    @Green.setter
    def Green(self, value: int) -> int: ...
    @property
    def Red(self) -> int: 
        """Red component."""
        ...
    @Red.setter
    def Red(self, value: int) -> int: ...
    def Deconstruct(self, red: clr.Reference[int], green: clr.Reference[int], blue: clr.Reference[int], clear: clr.Reference[int]) -> None: 
        """Unpacks RGBC tuple."""
        ...


class RobotIO(IDisposable):
    """The main class through which input and output operations are performed with devices and components connected to the Arduino."""
    @typing.overload
    def __init__(self, port: str) -> None: ...
    @typing.overload
    def __init__(self, port: str, baud: int, dtrEnable: bool) -> None: ...
    @property
    def Analog(self) -> Analog: 
        """Allows for configuring of analog input, and exposing analog readings."""
        ...
    @property
    def ColourSensor(self) -> ColourSensor: 
        """Reports values read using TCS34725 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
        ...
    @property
    def ConnectionState(self) -> ConnectionState: 
        """Informs the connected state of the computer with the Arduino."""
        ...
    @property
    def DHTSensor(self) -> DHTSensor: 
        """Reports temperature and humidity values using a DHT20 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
        ...
    @property
    def Digital(self) -> Digital: 
        """Exposes digital input readings."""
        ...
    @property
    def Display(self) -> DisplayLcd: 
        """Sends text output to the LCD display."""
        ...
    @property
    def LedDisplay(self) -> DisplayLed: 
        """Writes a string to the 7 segment LED display."""
        ...
    @property
    def LightMeter(self) -> LightMeter: 
        """Reports values using the BH1750 light meter sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
        ...
    @property
    def Motor1(self) -> Motor: 
        """Controls speed and direction of motor 1."""
        ...
    @property
    def Motor2(self) -> Motor: 
        """Controls speed and direction of motor 2."""
        ...
    @property
    def MotorConfig(self) -> MotorConfig: 
        """Configuration options for DC motors."""
        ...
    @property
    def MPUSensor(self) -> MPUSensor:
        """Reports accelerometer and gyrometer readings from an MPU6050 sensor. This feature is only available in the Education Edition of the Robo-Tx API."""
        ...
    @property
    def PulseCounter(self) -> PulseCounter:
        """Calculates period of input pulses on a designated Arduino pin. This feature is only available in the Education Edition of the Robo-Tx API."""
        ...
    @property
    def RobotId(self) -> str: 
        """Gets the user defined ROBOT_ID string set in the firmware file Settings.h. Use the value of this property to distinguish between different robots connected at the same time."""
        ...
    @RobotId.setter
    def RobotId(self, value: str) -> str: ...
    @property
    def Servo1(self) -> Servo: 
        """Sets the range and position of servo motor 1."""
        ...
    @property
    def Servo2(self) -> Servo: 
        """Sets the range and position of servo motor 2."""
        ...
    @property
    def Servo3(self) -> Servo: 
        """Sets the range and position of servo motor 3."""
        ...
    def Servo4(self) -> Servo: 
        """Sets the range and position of servo motor 4."""
        ...
    @property
    def Servo5(self) -> Servo: 
        """Sets the range and position of servo motor 5."""
        ...
    @property
    def Servo6(self) -> Servo: 
        """Sets the range and position of servo motor 6."""
        ...
    @property
    def ServoConfig(self) -> ServoConfig: 
        """Configuration options for servo motors."""
        ...
    @property
    def Sonar(self) -> Sonar: 
        """Uses sonar to calculate distance by sending a ping and measuring the time lapsed before receiving the echo."""
        ...
    @property
    def Switch1(self) -> Switch: 
        """Sets a digital output to on or off."""
        ...
    @property
    def Switch2(self) -> Switch: 
        """Sets a digital output to on or off."""
        ...
    @property
    def Switch3(self) -> Switch: 
        """Sets a digital output to on or off."""
        ...
    @property
    def Switch4(self) -> Switch: 
        """Sets a digital output to on or off."""
        ...
    @property
    def Trigger(self) -> Trigger: 
        """A digital trigger for repeating pulse cycle patterns."""
        ...
    def Close(self) -> None: 
        """Closes connection with Arduino. DC motors and servo motors will be disabled, the digital trigger and digital switches will turn off, and the LED display will clear."""
    def Connect(self) -> None: 
        """Initiates serial connection with Arduino using port and baud rates specified in constructor."""
        ...
    def Dispose(self) -> None: 
        """Closes connection with the Arduino and releases resources."""
        ...
    def NotifyClosing(self) -> None: 
        """The method should be called in the main application to signal to background threads monitoring property"""
        ...
    # Skipped WaitUntilSensorsReady due to it being static, abstract and generic.

    WaitUntilSensorsReady : WaitUntilSensorsReady_MethodGroup
    """Waits up to the timeout period for specified sensors to become ready. The sensors must have been enabled beforehand. Analog sensors are enabled by default."""
    class WaitUntilSensorsReady_MethodGroup:
        @typing.overload
        def __call__(self, sensors: Array_1[ContinuousSensor]) -> bool:...
        @typing.overload
        def __call__(self, timeoutMs: int, sensors: Array_1[ContinuousSensor]) -> bool:...



class Servo:
    """Configures the range and the position setting of a servo motor. The Arduino pins assigned for servo motors are configured in the firmware settings."""
    @property
    def Position(self) -> float: 
        """Gets the current position angle of the servo that was set using SetPosition(Single). The value returned is not guaranteed to reflect the actual physical position of the servo, since it can take time for the servo to move to a given position, or the servo may have been manually re-positioned whilst in a stopped state."""
        ...
    @property
    def Range(self) -> ServoRange: 
        """Gets the servo range settings."""
        ...
    def SetPosition(self, angle: float) -> None: 
        """Sets the position angle of a specified servo motor."""
        ...
    def SetSpeed(self, speed: int) -> None: 
        """Sets the speed with which the servo moves to a specified position. The actual speed and range of speed will depend on the type of servo used. By default the servo is set to move at maximum speed."""
        ...
    def Stop(self) -> None: 
        """Stops the specified servo so it no longer maintains its position."""
        ...


class ServoConfig:
    """Configuration options for servo motors."""
    def SetAngleLimits(self, angleLowerLimit: int, angleUpperLimit: int, servo: Servo) -> None: 
        """Sets the servo angle lower and upper limits to prevent the servo travelling beyond physical boundaries."""
        ...
    def SetSpeedLimit(self, maxSpeed: int, servo: Array_1[Servo]) -> None: 
        """Sets the maximum speed of the servos."""
        ...
    def SetType(self, maxAngle: int, minPulseWidth: int, maxPulseWidth: int, servo: Array_1[Servo]) -> None: 
        """Sets the type of specified servo motor in terms of minimum and maximum pulse width to ensure correct positioning."""
        ...


class ServoRange:
    """Maintains the range settings for a servo."""
    @property
    def LowerLimit(self) -> int: 
        """Gets the angle lower limit set for the servo."""
        ...
    @LowerLimit.setter
    def LowerLimit(self, value: int) -> int: ...
    @property
    def MaxAngle(self) -> int: 
        """Gets the maximum physical angle of the servo."""
        ...
    @MaxAngle.setter
    def MaxAngle(self, value: int) -> int: ...
    @property
    def UpperLimit(self) -> int: 
        """Gets the angle upper limit set for the servo."""
        ...
    @UpperLimit.setter
    def UpperLimit(self, value: int) -> int: ...


class Sonar:
    """Uses sonar to calculate distance by sending a ping and measuring the time lapsed before receiving the echo. The maximum distance that can be measured is 165 Centimetres. Sonar works best to detect objects with hard surfaces that reflect sound well. The sonar sensor may detect a closer object off to the side instead of a farther object straight ahead. The Arduino pins assigned for the sonar module are configured in the firmware settings."""
    @property
    def DistanceAcquired(self) -> bool: 
        """Returns true if a distance value has been calculated from the sonar echo. Returns false if no echo was received in time."""
        ...
    def Clear(self) -> None: 
        """Clears the current distance value."""
        ...
    def GetDistance(self) -> int: 
        """Gets the distance calculated from the sonar echo after calling Ping(). A value of -1 indicates no echo was received in time. Note that a call to this method will reset the distance to -1. Therefore, the caller should store the value returned in a variable."""
        ...
    def Ping(self) -> None: 
        """Sends one ping using a sonar module."""
        ...


class Switch:
    """Sets a digital output to on or off. The Arduino digital pin and values that represent on and off are configured in the firmware settings."""
    @property
    def DurationLapsed(self) -> bool: 
        """Returns true if a previously set duration in seconds for the output has lapsed. Returns false otherwise."""
        ...
    @property
    def IsOn(self) -> bool: ...
    @IsOn.setter
    def IsOn(self, value: bool) -> bool: 
        """Returns true if the switch is set to On, false otherwise...."""
    def Off(self) -> None: 
        """Switches off the digital output."""
        ...
    def On(self) -> None: 
        """Switches on the digital output."""
        ...
    def OnForDuration(self, duration: float) -> None: 
        """Switches on the digital output for the specified duration in seconds."""
        ...


class TCS34725(abc.ABC):
    """Symbolic labels for colour sensor configuration settings."""
    GAIN_16X : int
    """Represents sensor gain of 16x."""
    GAIN_1X : int
    """Represents sensor gain of 1x."""
    GAIN_4X : int
    """Represents sensor gain of 4x."""
    GAIN_60X : int
    """Represents sensor gain of 60x."""
    INTEGRATION_TIME_101MS : int
    """Represents integration time of 101 ms."""
    INTEGRATION_TIME_154MS : int
    """Represents integration time of 154 ms."""
    INTEGRATION_TIME_2_4MS : int
    """Represents integration time of 2.4 ms."""
    INTEGRATION_TIME_24MS : int
    """Represents integration time of 24 ms."""
    INTEGRATION_TIME_50MS : int
    """Represents integration time of 50 ms."""
    INTEGRATION_TIME_700MS : int
    """Represents integration time of 700 ms."""


class Trigger:
    """A digital trigger for repeating pulse cycle patterns. Use for an audio or visual alert (e.g. active beeper) using repeating cycle patterns. The Arduino digital pin and values that represent on and off are configured in the firmware settings."""
    @property
    def IsActive(self) -> bool: 
        """Returns true if the trigger is on or running a pattern cycle. Returns false otherwise."""
        ...
    def Off(self) -> None: 
        """Stops the trigger if it is on or running a pattern cycle."""
        ...
    def On(self) -> None: 
        """Switches on the trigger for an indefinite period."""
        ...
    def Repeat(self, onPeriod: int, offPeriod: int) -> None: 
        """Pulses the trigger indefinitely with a continous sequence of on/off periods."""
        ...
    def RunPattern(self, onPeriod: int, offPeriod: int, cycles: int, loopCycles: int = ..., loopDelayPeriod: int = ...) -> None: 
        """Runs a repeating pattern of digital pulses."""
        ...
    # Skipped Pulse due to it being static, abstract and generic.

    Pulse : Pulse_MethodGroup
    """Trigger a short pulse."""
    class Pulse_MethodGroup:
        @typing.overload
        def __call__(self) -> None:...
        @typing.overload
        def __call__(self, onPeriod: int) -> None:...

    # Skipped SetOffPeriod due to it being static, abstract and generic.

    SetOffPeriod : SetOffPeriod_MethodGroup
    """Sets the off period between pulses whilst the trigger is cyclng through a repeat pattern."""
    class SetOffPeriod_MethodGroup:
        def __call__(self, offPeriod: float) -> None:...
        # Method SetOffPeriod(offPeriod : Int32) was skipped since it collides with above method
