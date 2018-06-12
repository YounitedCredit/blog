# Younited.Scheduling

This folder contains the source code of a system that illustrates how to implement a minimalistic scheduling system based on Azure Service Bus queues. This system relies on two applications:

* A publisher, which is responsible for scheduling messages on the queue.
* A subscriber, implemented as an Azure Function, that listens to the queue and is triggered every time a message becomes available.

Before running these two applications, you will need an Azure Service Bus namespace (see [this doc](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal) for more information).

Once you've got the connection settings of the namespace:

* Connect to it and create a message queue named `pending_tasks`.
* Set these connection settings in the configuration files `Younited.Scheduling.Publisher\appsettings.json` and `Younited.Scheduling.Subscriber\local.settings.json`.
* Build the projects and run them!
