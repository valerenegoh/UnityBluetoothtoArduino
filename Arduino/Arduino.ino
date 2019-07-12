char incomingData;
int C1_LED = 10;
int D1_LED = 11;
int E1_LED = 12;
int F1_LED = 13;
int G1_LED = 14;
int A1_LED = 15;
int B1_LED = 16;
int C2_LED = 2;
int D2_LED = 3;
int E2_LED = 4;
int F2_LED = 5;
int G2_LED = 6;
int A2_LED = 7;
int B2_LED = 8;
int C3_LED = 9;

void setup() {
  Serial.begin(9600);
  pinMode(C1_LED, OUTPUT);
  pinMode(D1_LED, OUTPUT);
  pinMode(E1_LED, OUTPUT);
  pinMode(F1_LED, OUTPUT);
  pinMode(G1_LED, OUTPUT);
  pinMode(A1_LED, OUTPUT);
  pinMode(B1_LED, OUTPUT);
  pinMode(C2_LED, OUTPUT);
  pinMode(D2_LED, OUTPUT);
  pinMode(E2_LED, OUTPUT);
  pinMode(F2_LED, OUTPUT);
  pinMode(G2_LED, OUTPUT);
  pinMode(A2_LED, OUTPUT);
  pinMode(B2_LED, OUTPUT);
  pinMode(C3_LED, OUTPUT);
  digitalWrite (C1_LED, LOW);
  digitalWrite (D1_LED, LOW);
  digitalWrite (E1_LED, LOW);
  digitalWrite (F1_LED, LOW);
  digitalWrite (G1_LED, LOW);
  digitalWrite (A1_LED, LOW);
  digitalWrite (B1_LED, LOW);
  digitalWrite (C2_LED, LOW);
  digitalWrite (D2_LED, LOW);
  digitalWrite (E2_LED, LOW);
  digitalWrite (F2_LED, LOW);
  digitalWrite (G2_LED, LOW);
  digitalWrite (A2_LED, LOW);
  digitalWrite (B2_LED, LOW);
  digitalWrite (C3_LED, LOW);
}

void loop() {
  if(Serial.available()>0){
    incomingData = Serial.read();
    Serial.write(incomingData);
    switch(incomingData){
      case '0':
        digitalWrite (C1_LED, HIGH);
        Serial.println("C");
        delay(100);
        digitalWrite(C1_LED, LOW);
        break;
      case '1':
        digitalWrite (D1_LED, HIGH);
        Serial.println("D");
        delay(100);
        digitalWrite(D1_LED, LOW);
        break;
      case '2':
        digitalWrite (E1_LED, HIGH);
        Serial.println("E");
        delay(100);
        digitalWrite(E1_LED, LOW);
        break;
      case '3':
        digitalWrite (F1_LED, HIGH);
        Serial.println("F");
        delay(100);
        digitalWrite(F1_LED, LOW);
        break;
      case '4':
        digitalWrite (G1_LED, HIGH);
        Serial.println("G");
        delay(100);
        digitalWrite(G1_LED, LOW);
        break;
      case '5':
        digitalWrite (A1_LED, HIGH);
        Serial.println("A");
        delay(100);
        digitalWrite(A1_LED, LOW);
        break;
      case '6':
        digitalWrite (B1_LED, HIGH);
        Serial.println("B");
        delay(100);
        digitalWrite(B1_LED, LOW);
        break;
      case '7':
        digitalWrite (C2_LED, HIGH);
        Serial.println("C");
        delay(100);
        digitalWrite(C2_LED, LOW);
        break;
      case '8':
        digitalWrite (D2_LED, HIGH);
        Serial.println("D");
        delay(100);
        digitalWrite(D2_LED, LOW);
        break;
      case '9':
        digitalWrite (E2_LED, HIGH);
        Serial.println("E");
        delay(100);
        digitalWrite(E2_LED, LOW);
        break;
      case 'a':
        digitalWrite (F2_LED, HIGH);
        Serial.println("F");
        delay(100);
        digitalWrite(F2_LED, LOW);
        break;
      case 'b':
        digitalWrite (G2_LED, HIGH);
        Serial.println("G");
        delay(100);
        digitalWrite(G2_LED, LOW);
        break;
      case 'c':
        digitalWrite (A2_LED, HIGH);
        Serial.println("A");
        delay(100);
        digitalWrite(A2_LED, LOW);
        break;
      case 'd':
        digitalWrite (B2_LED, HIGH);
        Serial.println("B");
        delay(100);
        digitalWrite(B2_LED, LOW);
        break;
      case 'e':
        digitalWrite (C3_LED, HIGH);
        Serial.println("C");
        delay(100);
        digitalWrite(C3_LED, LOW);
        break;
    }
  }
}
