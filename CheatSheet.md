## Ch1. The basics:
### Reponse to HTTP requests:
### Nuget Leverage External Dependencies:
### Static Files:
### Error Handling and Diagnostics:
### Custom Configuration Library - Dependency (Built in from MS):
### Populate Config Setting - from json file:
### Increase Maintainability with Dependency Injection:
o Low-level APIs
o Simple Logic 
o Static Files
### DONE WITH CHAPTER 1! TIME FOR FOOD.

Short cut and tricks:
```
Create a constructor:
    ctor -> tab twice
    public Startup()
    { // more code here ... }

Quick actions / refactorings:
    Ctrl + .


```
## Ch2. The MVC Pattern:
### Understand the MVC Pattern:
ASP.NET Core MVC Framework:
```
o Named for Model-View-Controller (MVC) pattern.
o User interface design pattern.
o Promotes separation of concerns across multiple application layers.
```
Instead for putting the View, Application, Business Logic in the same place.
MVC separated these into specific classes.
```
Business Logic:     Model
View Logic:         View
Application Logic:  Controller
```
View: 
```
o Interact with the users.
o HTML, CSS, JavaScript.
o Delivery high good-looking user experience to the user.
o Make it pretty, leave the functionality to the controllers and the models, just display the result.
o The view may call back to the controller to execute logic that's outside of the view's responsibilities.
```
Controller:
```
o This is the traffic cop of the application, controll the workflow as the user interact with the view.
o The controller orchestrates interaction between the model and the view.
```
^NOTE: Don't put all the application logic in the controller, put them in the model, the controller should have only the orchestration logic.

Model:
```
o Data structures.
o This is the heart of the application.
o All the business logic are here, ie: 
    bussiness processes
    validation rules
    system integration with other system
o Does not know anything about the VIEW or the CONTROLLER 
```
NOTE: ^ Above are the .NET MVC pattern, so what is ASP.net CORE MVC?
```
o MS implementation of the MVC design pattern on top of the ASP.net CORE platform.
o A framework that built on top of those core building blocks, help to create a website.
```
