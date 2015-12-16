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
      
    }
  }
}

void mainProgram()
{
  
}

void returnValues()
{
  
}

void resetAll()
{
  mySwitch.send(Freq[5][0], 24);
  delay(5000);
  mySwitch.send(Freq[5][0], 24);
  delay(5000);
}

