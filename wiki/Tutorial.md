### Installieren

**1. Repository clonen:**  
``git clone https://github.com/juliankeppler/sweproject``

**2. Installationsordner öffnen:**  
``cd sweproject``

**3. Dependencies installieren:**  
``dotnet restore``

### Ausführen

**1. Das Programm ausführen:**  
Wenn Make installiert ist kann das Programm mit

``make start``

ausgeführt werden. Alternativ kann es auch mit folgendem Befehl aufgerufen werden:

``dotnet run -p src``

**2. Das gefragte Stichwort eingeben:**  
``Bitte geben sie einen Suchbegriff ein:``  
z.B.``Corona``

**3. Den gefragten Zeitraum eingeben:**  
``Bitte geben sie eine oder mehrere Legislaturperioden ein (Bsp. '19' oder '17, 18, 19'). Wir befinden uns derzeit in der 19. Legislaturperiode:``  
z.B.``19``<br>Wenn dieses Feld freigelassen wird, versucht das Programm ein Diagramm für alle im Archiv vorhandenen Ergebnisse zum Suchbegriff zu erstellen.

Das erstellen des Diagramms kann einen Moment beanspruchen...  

**4. Das erstellt Diagramm öffnen:**  
``graph.png``
<div align="left"><img src="https://raw.githubusercontent.com/juliankeppler/sweproject/main/docs/example_corona.png" alt="Beispieldiagramm Corona" width="450"/></div>

**5. Um weitere Parameter auszuprobieren einfach erneut das Programm ausführen**

**Achtung:** Jedes neu erstellte Diagramm überschreibt eventuell vorhandene ältere Diagramme

Bei Eingabe einer einzelnen Legislaturperiode als Zeitraum läuft das Programm im Monat-Modus, bei Eingabe mehrerer Perioden im Jahr-Modus.