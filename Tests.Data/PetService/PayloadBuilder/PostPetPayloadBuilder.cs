using Models.PetService.Payload;
using System;
using System.Collections.Generic;

namespace Tests.Data.PetService.PayloadBuilder
{
    /// <summary>
    /// Builder class for dynamically constructing payload for CreatePetRequest
    /// </summary>
    public class PostPetPayloadBuilder
    {
        private long id = GenerateId();
        private string name = $"qa-pet-{GenerateId()}";
        private List<string> photoUrls = ["https://example.com/default.jpg"];
        private string status = "available";

        private static long GenerateId()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public PostPetPayloadBuilder SetId(long id)
        {
            this.id = id;
            return this;
        }

        public PostPetPayloadBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public PostPetPayloadBuilder SetPhotoUrls(List<string> photoUrls)
        {
            this.photoUrls = photoUrls ?? [];
            return this;
        }

        public PostPetPayloadBuilder AddPhotoUrl(string url)
        {
            photoUrls ??= [];
            photoUrls.Add(url);
            return this;
        }

        public PostPetPayloadBuilder SetStatus(string status)
        {
            this.status = status;
            return this;
        }

        public PostPetPayload Build()
        {
            return new PostPetPayload
            {
                Id = id,
                Name = name,
                PhotoUrls = photoUrls,
                Status = status
            };
        }
    }
}