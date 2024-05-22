GarageDoorsWeb
Overview

GarageDoorsWeb is a web application designed to remotely open and close garage doors. The system uses an Arduino to control relays, which act as button remotes for the garage doors. The web application is hosted on a Raspberry Pi 4, and the domain is purchased from Namecheap. A mobile app development is also planned for the future.
Features

    Remotely control garage doors from a web application
    Secure login with JWT authentication
    Real-time status updates of the garage doors
    Hosted on Raspberry Pi 4
    Mobile app development planned

Architecture

Components

    Web Application
        Built using ASP.NET Core
        Secure login using JWT tokens
        Hosted on a Raspberry Pi 4

    Arduino Controller
        Uses ESP8266 for WiFi connectivity
        Controls relays which act as button remotes for the garage doors
        Communicates with the web application to get door statuses and trigger relays

    Domain
        Purchased from Namecheap

    Raspberry Pi 4
        Hosts the web application
