#include <RCSwitch.h>
#include <Ethernet.h>
#include <SPI.h>

byte mac[] = {0x40, 0x6e, 0x9f, 0x06, 0xe4, 0x7a};
IPAddress ip(192, 168, 1, 10);
RCSwitch mySwitch = RCSwitch();
EthernetServer server(32545);
bool connected = false;

const float Freq[][2] = {{8262702, 8262703}, {8262700, 8262701}, {8262698, 8262699}, {8262694, 8262695}, {8262689, 8262690}};

bool States[] = {false, true, false, false};



void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  mySwitch.enableTransmit(5);
  setAll(0);

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

  while (ethernetClient.connected())
  {
    //Serial.println("Application connected");
    char buffer[128];
    int count = 0;
    while(ethernetClient.available())
    {
      buffer[count++] = ethernetClient.read();
    }
    buffer[count] = '\0';

    if(count > 0)
    {
      Serial.println(buffer);
      //Channel 1
      if(String(buffer) == String("Ch1ON")) setSwitch(1, 1);
      else if(String(buffer) == String("Ch1OFF")) setSwitch(1, 0);
      //Channel 2
      if(String(buffer) == String("Ch2ON")) setSwitch(2, 1);
      else if(String(buffer) == String("Ch2OFF")) setSwitch(2, 0);
      //Channel 3
      if(String(buffer) == String("Ch3ON")) setSwitch(3, 1);
      else if(String(buffer) == String("Ch3OFF")) setSwitch(3, 0);
      //Channel 4
      if(String(buffer) == String("Ch4ON")) setSwitch(4, 1);
      else if(String(buffer) == String("Ch4OFF")) setSwitch(4, 0);
      //Channel All
      if(String(buffer) == String("ChAllON")) setAll(1);
      else if(String(buffer) == String("ChAllOFF")) setAll(0);
      //Return States to app
      if(String(buffer) == String("States")) returnStates(ethernetClient);
      //Return stuff
      //if(String(buffer) == String("getVal")) returnValues();
      //
    }
  }
}

void returnValues()
{
  
}

void setSwitch(int adapter, int state)
{
  Serial.print("Turning switch ");
  Serial.print(adapter);
  Serial.print(" ");
  Serial.println(state);
  mySwitch.send(Freq[adapter - 1][state], 24);
  if(state == 0) States[adapter - 1] = false;
  else States[adapter - 1] = true;
  delay(100);
}

void setAll(int state)
{
  for(int i = 0; i < 4; i++)
  {
    if(state == 0) States[i] = false;
    else States[i] = true;
  }
  mySwitch.send(Freq[4][state], 24);
  delay(100);
}

void returnStates(EthernetClient client)
{
  String bools = "";
  for(int i = 0; i < 4; i++)
  {
    String temp;
    if(States[i]) temp = "true";
    else temp = "false"; 
    bools = String(bools + temp);
    if(i < 3) bools = String(bools + ",");
  }
  Serial.println(bools);
  client.print(bools);
}

