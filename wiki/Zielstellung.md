### Zielstellung des Projekts
Ziel ist es, eine grafische Darstellung der zeitabhängigen Relevanz bestimmter Themen im Diskurs des deutschen Bundestags bereitzustellen.  
(à la Google Trends)

![](https://cdn.discordapp.com/attachments/435461260004425740/861247865706643456/unknown.png)

### Umsetzung
Das *Dokumentations- und Informationssystem für Parlamentsmaterialien* DIP verfügt über eine API mit welcher sämtliche Plenarprotokolle des Bundestags automatisch abgerufen und durchsucht werden können.  
Mit dieser API können wir die Protokolle nach Redebeitägen durchsuchen, welche, vom Benutzer gewählte, Schlagwörter enthalten.  
Anhand des Datums jedes Protokolls können wir genau feststellen, wie viele Reden in einem bestimmten Zeitraum zu einem bestimmten Thema gehalten wurden und diese Werte, zum Beispiel mit ScottPlot, als Diagramm darstellen.

### Zeitpriorisierung
Umsetzungsziel:
- Einbindung DIP API
- Implementierung ScottPlot (Ausgabe als Datei)

Falls noch Zeit übrig ist:
- Vergleichen zweier Begriffe
- Grafische Benutzeroberfläche für Ein- und Ausgabe
