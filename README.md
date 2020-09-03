![](Images/go.png?raw=true "GoBabyGo Logo")

## Synopsis
Being apart of the Student IEEE Chapter at Central Connecticut State University, one of the ongoing projects 
we hope to finish very soon is called GoBabyGo.

<b>Go Baby Go</b> is a national organization that enables mobility solutions for children with disabilities.

<b>Our</b> clubs goal is to implement advanced electronic solutions in the form of a customizable, 
remotely controlled car, for movement facilitation.

This Git project is a <b>Xamarin Forms Mobile</b> app for both Android and iOS for controlling the car using Bluetooth 4.0 LE for communication:
1. From the car to the app (Sender) 
2. From the app to the car (Receiver)

The car will send status messages to the phone about its own sensors. 
And the phone will send messages to the car with commands for controlling the motors.

Tilting the phone down is reverse, up is forward, and logically, left and right control turning depending on the orientation of the phone.


## Project Dev

### App Design

Welcome Page | Control Page |
:------------------------------------------------:|:-------------------------------------------------:
![](Images/welcomepage.jpg?raw=true "UI Design")  |  ![](Images/controlpage.jpg?raw=true "UI Design")

---

### Branching

The ```master``` branch contains the latest stable build of the project.
---

### NuGet Packages Used
- [Xamarin.Essentials](https://github.com/xamarin/Essentials) - A kit of essential API's!
- [MVVM Helpers](https://github.com/jamesmontemagno/mvvm-helpers) - MVVM helper classes!
- [Xamarin.Forms.InputKit](https://github.com/enisn/Xamarin.Forms.InputKit) -  Awesome Controls such as Checkbox, Radiobutton, etc!