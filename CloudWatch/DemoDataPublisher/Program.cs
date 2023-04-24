using System.Text.Json;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using AWS.Logger;
using AWS.Logger.Core;
using DemoDataPublisher.Models;

var cwClient = new AmazonCloudWatchLogsClient();

const string awsCloudWatchLogGroupName = "logging-in-aws";

var serviceNames = new List<string> { "Admin", "Search", "Booking" };
var envNames = new List<string> { "Staging", "Prod" };
var exceptions = new List<Exception>
{
    new InvalidParameterException("The provided parameter is invalid"),
    new JsonException("The provided Json cannot be de-serialized",
        new FileLoadException("File not found"))
};

var logGroupsResponse = await cwClient.DescribeLogGroupsAsync(new DescribeLogGroupsRequest
{
    Limit = 1,
    LogGroupNamePattern = awsCloudWatchLogGroupName
});

if (!logGroupsResponse.LogGroups.Any())
    await cwClient.CreateLogGroupAsync(new CreateLogGroupRequest(awsCloudWatchLogGroupName));
var logger = new AWSLoggerCore(new AWSLoggerConfig(awsCloudWatchLogGroupName), "fail");

for (var i = 0; i < 100; i++)
{
    var seed = (int)DateTime.Now.Ticks;
    var logModel = new DemoEventModel
    {
        ServiceName = serviceNames[new Random(seed).Next(2)],
        Environment = envNames[new Random(seed).Next(1)],
        Exception = exceptions[new Random(seed).Next(2)]
    };
    logger.AddMessage(JsonSerializer.Serialize(logModel));
}


Console.WriteLine("100 log entries were sent to Cloudwatch.");