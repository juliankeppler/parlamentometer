<div align="center"><img src="https://user-images.githubusercontent.com/73278698/127150299-2430237a-eac3-43c0-b075-98a91de85cb7.png") alt="Beispieldiagramm Corona" width="600"/></div>

<div align="center"><img src="https://user-images.githubusercontent.com/73278698/127149208-97f3f66d-9570-4172-94ec-e500796b90ba.png") alt="Beispieldiagramm Corona" width="600"/>
  
### Welche Themen treiben den Deutschen Bundestag um?
  
<a href='https://coveralls.io/github/juliankeppler/sweproject?branch=main'><img src='https://coveralls.io/repos/github/juliankeppler/sweproject/badge.svg?branch=main' alt='Coverage Status' /></a>
</div>

#

Unser Programm macht es möglich, die Relevanz einzelner Themen im politischen Diskurs des Deutschen Bundestags zu visualisieren, aktuell und historisch.

Mithilfe des [Dokumentations- und Informationssystems für Parlamentsmaterialien](https://dip.bundestag.de/) (kurz *DIP*) und dessen [API](https://dip.bundestag.de/%C3%BCber-dip/hilfe/api), können wir Plenarprotokolle des Deutschen Bundestags von ca. 1976 bis heute durchsuchen und abrufen. 

Wir nutzen diese Funktion, um nachzuverfolgen, wie oft im Plenarsaal Reden gehalten werden oder wurden, welche vom Benutzer gewählte Schlagwörter enthalten. Diesen Wert stellen wir in Abhängigkeit der Zeit grafisch dar.

## Funktion
Die Stichwörter/Themen können vom Benutzer frei gewählt werden, ebenso kann der zu betrachtende Zeitraum eingegrenzt werden.  
So kann man einen Trend im Verlauf eines Jahres oder aber über viele Jahre hinweg verfolgen.  
Visuell orientiert sich unsere grafische Darstellung an [Google Trends.](https://trends.google.com/trends/) 

*Beispieldiagramm zum Suchbegriff "Corona" während der 19. Legislaturperiode:*
<div align="center"><img src="https://raw.githubusercontent.com/juliankeppler/sweproject/main/docs/example_corona.png" alt="Beispieldiagramm Corona" width="800"/></div>

Zeitraum und Suchbegriff werden über die Kommandozeile abgefragt und eingegeben, wobei der Suchzeitraum in Legislaturperioden angegeben wird.   
Das resultierende Diagramm wird unter dem Namen graph.png im Installationsordner des Programms als Bild gespeichert.

Der Fokus liegt dabei eher darauf, einen groben Überblick über längerfristige Entwicklungen des politischen Diskurses im Bundestag zu gewinnen, als auf der Genauigkeit der Werte.
## Struktur
Die Erstellung der URL für die Verwendung der API, sowie die Suche wird in **DIP.cs** abgewickelt.  
**Plot.cs** enthält den notwendigen Code für die Ausführung der grafischen Darstellung mit ScottPlot.  
**Program.cs** enthält die Main Methode, verknüpft die Dateien miteinander und führt sie aus.

- Eine bessere Übersicht über den Aufbau des Programms bieten die [UML-Diagramme.](https://github.com/juliankeppler/sweproject/wiki/UML)  
- Einen tiefgehenden Blick hinter die Kulissen verschafft die [Dokumentation.](https://github.com/juliankeppler/sweproject/wiki/documentation)

## Installation
Step-by-step Instruktionen zu Download und Verwendung finden Sie [hier.](https://github.com/juliankeppler/sweproject/wiki/Tutorial)

Systemvorraussetzungen:
- .NET 5.0

Das Repository klonen:

`git clone https://github.com/juliankeppler/sweproject`

Dependencies installieren:

`dotnet restore`

Das Programm ausführen:

`make start` oder `dotnet run -p src`

## Documentation

[All you need to know about the inner workings (in english).](https://github.com/juliankeppler/sweproject/wiki/documentation)


<br><br><br><br>
<div align="center">
(-__-)<br>
-<>-</div>
