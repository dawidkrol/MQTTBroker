﻿namespace MQTTBroker.AppCore.Services.Interface;

internal interface IMessageSender
{
    void PublishMessage(string topicName, string message);
}