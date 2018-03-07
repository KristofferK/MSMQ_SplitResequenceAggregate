# MSMQ_SplitResequenceAggregate
Exercise at academy.

The solution must be able to
* Open XML file
* Send the content of the XML file to another subsystem.
* This subsystem will split the message into passenger information and luggage information. Passenger information will be sent in one queue, while luggage information will be sent in another queue. The luggage information can be 1..n messages. These will me sent in a random order.
* The luggage information will have to be resequenced, so they no longer are in a random order. After resequencing, it will be sent to a subsystem that prints the information.
* The passager information will be sent to the subsystem that prints directly. No resequencing will be necessary here.
* When the printing subsystem has printed all the information, it will aggregate the passenger information and luggage information into one message.
* This message will be sent to yet another subsystem.

The solution will consist of five subsystems
* SubsystemA. Reads XML file and sends the content to SubsystemB.
* SubsystemB. Receives the content of the XML file. Splits it into one passenger message, and n luggage messages. Passenger message is sent to SubsystemD. Luggage messages are sent in a random order to SubsystemC.
* SubsystemC. Receives luggage messages in a random order. It will resequence them into the right order. The messages are then sent to SubsystemD.
* SubsystemD. Receives passenger messages and luggage messages in a fixed order. It will print the messages. When all messages are received, they will be aggregated, and sent to SubsystemE.
* SubsystemE. Receives an aggregated message. Will print the message.

![Overview of the solution](https://i.gyazo.com/9c5bb4948c48c61c50fa0d7abc016e5c.png)