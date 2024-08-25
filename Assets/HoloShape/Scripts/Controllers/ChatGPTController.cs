using System.Collections;
using System.Collections.Generic;
using OpenAI;
using System.Threading.Tasks;
using UnityEngine;

public static class ChatGPTController
{
    private static OpenAIApi openAi = new OpenAIApi();

    private static List<ChatMessage> messages = new List<ChatMessage>();   

    public static async Task<string> AskChatGPT(string question) {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = question;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o-mini-2024-07-18";

        var response = await openAi.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0) {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            return chatResponse.Content;


        }

        return "";

    }
    
}
