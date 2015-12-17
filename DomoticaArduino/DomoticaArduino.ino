#include <RCSwitch.h>
#include <Ethernet.h>
#include <SPI.h>

byte mac[] = {0x40, 0x6c, 0x8f, 0x36, 0x84, 0x8a};
IPAddress ip(192, 168, 1, 10);
RCSwitch mySwitch = RCSwitch();
EthernetServer server(32545);
bool connected = false;

const float Freq[][2] = {{8262702, 8262703}, {8262700, 8262701}, {8262698, 8262699}, {8262694, 8262695}, {8262689, 8262699}};

bool States[] = {false, false, false, false};



void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  mySwitch.enableTransmit(5);
  resetAll();

  Ethernet.begin(mac, ip);
  Serial.print("Adress: ");
  Serial.println(Ethernet.localIP());
  server.begin();
  connected = true;

}

void loop() {
  // put your main code here, to run repeatedly:
  if(!connected) return;
  EthernetClient ethernetClient = server.available();

  Serial.println("Application connected");
  while (ethernetClient.connected())
  {
    char buffer[128];
    int count = 0;
    while(ethernetClient.available())
    {
      buffer[count++] = ethernetClient.read();
    }
    buffer[count] = '\0';

    if(count > 0)
    {
      //Channel 1
      if(String(buffer) == String("Ch1ON")) States[0] = true;
      else if(String(buffer) == String("Ch1OFF")) States[0] = false;
      //Channel 2
      if(String(buffer) == String("Ch2ON")) States[1] = true;
      else if(String(buffer) == String("Ch2OFF")) States[1] = false;
      //Channel 3
      if(String(buffer) == String("Ch3ON")) States[2] = true;
      else if(String(buffer) == String("Ch3OFF")) States[2] = false;
      //Channel 4
      if(String(buffer) == String("Ch4ON")) States[3] = true;
      else if(String(buffer) == String("Ch4OFF")) States[3] = false;
      //Channel All
      if(String(buffer) == String("ChAllON")) States[4] = true;
      else if(String(buffer) == String("ChAllOFF")) States[4] = false;
      //Return stuff
      //if(String(buffer) == String("getVal")) returnValues();
      //
      mainProgram();
    }
  }
}

void mainProgram()
{
  for(int i = 0; i < 5; i++)
  {
    if(States[i] = true) mySwitch.send(Freq[i][1], 24);
    else mySwitch.send(Freq[i][0], 24);
    delay(1000);
  }
}

void returnValues()
{
  
}

void resetAll()
{
  mySwitch.send(Freq[4][0], 24);
  delay(5000);
  mySwitch.send(Freq[4][0], 24);
  delay(5000);
}

