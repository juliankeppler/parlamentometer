### Installieren

**1. Repository clonen:**  
``git clone https://github.com/juliankeppler/sweproject``

**2. Installationsordner öffnen:**  
``cd sweproject``

**3. Dependencies installieren:**  
``dotnet restore``

### Ausführen

**1. Installationsordner öffnen** (falls nicht schon offen):  
``cd sweproject``

**2. Das Programm ausführen:**  
``dotnet run``

**3. Das gefragte Stichwort eingeben:**  
``Bitte geben sie einen Suchbegriff ein:``  
z.B.``Corona``

**4. Den gefragten Zeitraum eingeben:**  
``Bitte geben sie eine oder mehrere Legislaturperioden ein (Bsp. '19' oder '17, 18, 19'). Wir befinden uns derzeit in der 19. Legislaturperiode:``  
z.B.``19``  
kurz gedulden...  

**5. Das erstellt Diagramm öffnen:**  
``graph.png``
<div align="left"><img src="https://raw.githubusercontent.com/juliankeppler/sweproject/main/docs/example_corona.png" alt="Beispieldiagramm Corona" width="450"/></div>

**6. Um andere Parameter auszuprobieren einfach wieder**  
``dotnet run``

**Achtung:** Jedes neu erstellte Diagramm überschreibt eventuell vorhandene ältere Diagramme

Bei Eingabe einer einzelnen Legislaturperiode als Zeitraum läuft das Programm im Monat-Modus, bei Eingabe mehrerer Perioden im Jahr-Modus.