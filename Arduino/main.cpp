#include <Arduino.h>
#include <WiFi.h>
#include <WiFiMulti.h>
#include <HTTPClient.h>
#define USE_SERIAL Serial
WiFiMulti wifiMulti;

int num = 0;

int freq = 2000;  // 频率
int channel1 = 0; // 通道
int channel2 = 1;
int resolution = 8; // 分辨率
boolean pullFlag = false;
void setup()
{
  //  Serial.begin(115200);
  ledcSetup(channel1, freq, resolution); // 设置通道
  ledcSetup(channel2, freq, resolution);
  ledcAttachPin(5, channel1);  // 将通道与对应的引脚连接
  ledcAttachPin(14, channel2); // 将通道与对应的引脚连接
  USE_SERIAL.begin(115200);

  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();

  for (uint8_t t = 4; t > 0; t--)
  {
    USE_SERIAL.printf("[SETUP] WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }

  wifiMulti.addAP("tingjian", "13818611371");
}

void loop()
{
  if ((wifiMulti.run() == WL_CONNECTED))
  {
    int a = analogRead(35);
    num = map(a, 0, 4096, 0, 255);
    ledcWrite(channel1, num);
    if (a < 100)
    {
      ledcWrite(channel2, 0);
    }
    else
    {
      ledcWrite(channel2, 255);
    }
    USE_SERIAL.println(a);
    if (a > 2500)
    {
      pullFlag = true;
    }

    if (pullFlag == true && a == 0)
    {
      shoot();
      pullFlag = false;
    }
  }

  //  Serial.println(a);s
  delay(10);
}

void shoot()
{
  USE_SERIAL.print("shoot");
  HTTPClient http;
  USE_SERIAL.print("[HTTP] begin...\n");

  http.begin("http://192.168.199.168:5000/shoot"); // HTTP

  USE_SERIAL.print("[HTTP] GET...\n");
  // start connection and send HTTP header
  int httpCode = http.GET();

  // httpCode will be negative on error
  if (httpCode > 0)
  {
    // HTTP header has been send and Server response header has been handled
    USE_SERIAL.printf("[HTTP] GET... code: %d\n", httpCode);
    // file found at server
    if (httpCode == HTTP_CODE_OK)
    {
      String payload = http.getString();
      USE_SERIAL.println(payload);
    }
  }
  else
  {
    USE_SERIAL.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
  }
  http.end();

  delay(2000);
}