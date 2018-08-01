![Main screen](https://raw.githubusercontent.com/MikolajBalcerek/PodpisBio/GUI%2BStable/Dokumentacja/Raport%20Ko%C5%84cowy/oknopodpis.PNG)

## Podpis Biometryczny, PPB 2017 ##
Aplikacja stworzona w trakcie Poznańskich Praktych Badawczych 2017.  
Pozwala na tworzenie szczegółowego profilu użytkownika na podstawie unikatowych cech podpisu na tablecie. Zebrane dane służą do weryfikacji tożsamości klienta z wysoką skutecznością i prezentacji cech w formie wykresów. Zastosowano algorytm Dynamicznego Przekształcania Czasu, badania kształtu, rozmiaru, ilości oderwań, siły przyciśnięcia rysika, czasu, przyspieszenia i prędkości złożonego podpisu.  Stworzony prototyp dodatkowo obsługiwał różne rozmiary sygnatur i dostosowywał się do indywidualnego użytkownika, w celem wyłonienia najbardziej efektywnych dla niego metod.  
[Przeczytaj nasz raport końcowy](https://github.com/MikolajBalcerek/PodpisBio/blob/GUI%2BStable/Dokumentacja/Raport%20Ko%C5%84cowy/PPB_2017_Podpis_Biometryczny_Raport_Koncowy.pdf)

Projekt został wyróżniony nagrodą **“Najlepszy Projekt”** głosami uczestników stażu.

### Uruchomienie  ###
Wymagania:  
- Windows 10 Creators Update lub wyższy
- Visual Studio
- Moduł Universal Windows Apps do Visual Studio
- Moduł C# do Visual Studio
- Nuget Package Manager z połączeniem internetowym

Przed uruchomieniem projektu należy z głównego katalogu *Baza Danych* rozpakować znajdujące w paczce *.zip* pliki *.mdf* oraz *.ldf* i przekopiować je do ścieżki *RestService\App_Data*. 
W folderze Program znajdują się dwa projekty *sln*: *PodpisBio* oraz *RestService*.
Pierwszy należy uruchomić *RestService*.
  

## Biometric Signature Verification App, PPB 2017 ##
Created during Poznan Research Internship 2017.
The tablet app allowed to create detailed user profiles based on unique writer’s characteristics, employing deep handwriting analysis supplemented by external factors. Collected data was used to verify user’s identity with high accuracy and present results in real time. Dynamic Time Warping algorithm is implemented, alongside shape, surface area, number of strokes, strength, timing, acceleration and velocity analyses. The developed prototype additionally handles signatures’ size scaling and adjusts for each individual user to pick the most reliable identification methods.  
[Read our paper here (in Polish)](https://github.com/MikolajBalcerek/PodpisBio/blob/GUI%2BStable/Dokumentacja/Raport%20Ko%C5%84cowy/PPB_2017_Podpis_Biometryczny_Raport_Koncowy.pdf)

The project has been recognized as the **Best Project** during PPB 2017, as voted by participants.


### Running the app  ###
Dependencies:  
- Windows 10 Creators Update or higher
- Visual Studio
- Universal Windows Apps Visual Studio Module
- C# Visual Studio Module
- Nuget Package Manager with Internet access

First, unzip *.zip* file in */Baza Danych* folder.  
Move the unpacked *.mdf* and *.ldf* files to *RestService\App_Data*.  
There are two *sln* projects included: *PodpisBio* and *RestService*. Run *RestService* first.

  
![Graphs](https://raw.githubusercontent.com/MikolajBalcerek/PodpisBio/GUI%2BStable/Dokumentacja/Raport%20Ko%C5%84cowy/oknowykres.png)  

![Verification](https://raw.githubusercontent.com/MikolajBalcerek/PodpisBio/GUI%2BStable/Dokumentacja/Raport%20Ko%C5%84cowy/oknoweryfikacji.PNG)


### Team ###
* Mikołaj Balcerek - Project Leader
* Bartosz Hejduk
* Mieczysław Krawiarz
* Adam Kulczycki
* Mikołaj Pabiszczak
* Michał Szczepanowski
* Dawid Twardowski
* Adrianna Załęska  
