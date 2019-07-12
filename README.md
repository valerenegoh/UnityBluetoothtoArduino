# UnityBluetoothtoArduino
Stage 4 of SUTD Capstone 45 Pico Musical Engineering Installation project.<br/>
Refer to the following [link](https://youtu.be/EubplCl5Q8s) for a demo.

Unity directly reads the corresponding text file and sends the notes to play to an Arduino via Serial communication.

Bluetooth communication between Unity and Arduino: https://assetstore.unity.com/packages/tools/network/bluetooth-le-for-ios-tvos-and-android-26661
This plugin only works for Android and iOS devices and not PC.

The Arduino script requires you to set up an electronic circuit with 25 LEDs/solenoids/etc as shown in the video. Ensure they are wired to the corresponding pin numbers as in the Arduino file. An Arduino MEGA was used for this project as it has enough number of pin holes.

Steps:
1. Wire the Arduino circuit containing HM10 Bluetooth module and solenoids.
2. Connect to your Arduino and upload the sketch in Arduino folder. Ensure RXTX is disconnected during upload to avoid timeout error.

For Android devices [deploy on Windows PC]:
3. Open the Jukebox project under Build folder in Android Studio.
4. Connect to your Android phone, enable USB Debugging options and Bluetooth & run application on the device.

For iOS devices [delpoy on Mac PC]:
3. [to be updated]

To add new songs to the playlist:
1. Run Part 1 of https://github.com/ValereneGoh/MiditoArduino to convert to midi to interpretable text format files.
2. Add the text file to the folder under Assets > Scripts > txt.
3. Open Unity and manually create a new CD player and holder for the new song via the Prefab folder.
