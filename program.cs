using SignalWire.Relay;
using SignalWire.Relay.Calling;
using System;
using System.Collections.Generic;

namespace Calling_InboundCall
{
    internal class TestConsumer : Consumer
    {
        protected override void Setup()
        {
         // ENTER IN YOUR AUTH CREDS
            Project = "ENTER IN YOUR PROJ ID HERE ASDEE-QWEQWEQ-RRTRY";
            Token = "ENTER PROJ ID HERE PTdaf6a2203f03e081317b";
            Contexts = new List<string> { "test" };
        }

        // This is executed in a new thread each time, so it is safe to use blocking calls
        protected override void OnIncomingCall(Call call)
        {
            // Answer the incoming call, block until it's answered or an error occurs
            AnswerResult resultAnswer = call.Answer();

            if (!resultAnswer.Successful)
            {
                // The call was not answered successfully, stop the consumer and bail out
                Stop();
                return;
            }
            
            RecordAction actionRecord = call.RecordAsync(new CallRecord
            {
            Audio = new CallRecord.AudioParams
            {
            Direction = CallRecord.AudioParams.AudioDirection.both,
            InitialTimeout = 5,
            EndSilenceTimeout = 5,
            }
            });
            
            var resultPromptSpeech = call.PromptTTS("Please say something",
            new CallCollect()
            {
            Speech = new CallCollect.SpeechParams()
            });

            
            if (resultPromptSpeech.Successful)
            {
            Console.WriteLine($"Client request : '{resultPromptSpeech.Result}'");
            }


            // Hangup
            call.Hangup();
            Console.WriteLine("Call Ended");


            // Stop the consumer
            Stop();
        }
    }

    internal class Program
    {
        public static void Main()
        {
            // Create the TestConsumer and run it
            new TestConsumer().Run();

            // Prevent exit until a key is pressed
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
