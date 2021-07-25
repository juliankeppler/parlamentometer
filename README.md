# Parlament-O-Meter

## Welche Themen treiben den Deutschen Bundestag um?
Dieses Programm bietet eine Möglichkeit, die Relevanz einzelner Themen im politischen Diskurs zu visualisieren, aktuell und historisch.

Mithilfe des [Dokumentations- und Informationssystems für Parlamentsmaterialien](https://dip.bundestag.de/) (kurz *DIP*) und dessen API, können wir Plenarprotokolle des Deutschen Bundestags von ca. 1976 bis heute durchsuchen und abrufen. 

Wir nutzen diese Funktion, um nachzuverfolgen, wie oft im Plenarsaal Reden gehalten werden oder wurden, welche bestimmte Stichwörter enthalten. Diesen Wert stellen wir in Abhängigkeit der Zeit grafisch dar.

## Funktion
Die Stichwörter oder Themen können vom Benutzer frei gewählt werden, ebenso kann der zu betrachtende Zeitraum eingegrenzt werden.  
So kann man einen Trend im Verlauf eines Jahres oder aber über viele Jahre hinweg verfolgen. Visuell orientiert sich unsere grafische Darstellung an Google Trends. 

*Beispieldiagramm zum Suchbegriff "Corona" während der 19. Legislaturperiode:*
<div align="center"><img src="https://raw.githubusercontent.com/juliankeppler/sweproject/main/docs/example_corona.png" alt="Beispieldiagramm Corona" width="800"/></div>

Zeitraum und Suchbegriff werden über die Kommandozeile abgefragt und eingegeben, wobei der Suchzeitraum in Legislaturperioden angegeben wird.   
Das resultierende Diagramm wird unter dem Namen graph.png im Installationsordner des Programms als Bild gespeichert.

Der Fokus liegt dabei eher darauf einen groben Überblick über längerfristige Entwicklungen des politischen Diskurses im Bundestag zu gewinnen, als auf der Genauigkeit der Werte.
## Struktur
Die Erstellung der URL für die Verwendung der API sowie die Suche wird in **DIP.cs** abgewickelt.  
**Plot.cs** enthält den notwendigen Code für die Ausführung der grafischen Darstellung mit ScottPlot.  
**Program.cs** enthält die Main Methode, verknüpft die Dateien miteinander und führt sie aus.

Die genaue Struktur und Funktion des Programms wird in der [Dokumentation](https://github.com/juliankeppler/sweproject/wiki) erläutert. 


## Installation

Systemvorraussetzungen:
- .NET 5.0

Das Repository klonen:

`git clone https://github.com/juliankeppler/sweproject`

Dependencies installieren:

`dotnet restore`

Das Programm ausführen:

`dotnet run`

## Dokumentation

[Dokumentation im Wiki](https://github.com/juliankeppler/sweproject/wiki)
