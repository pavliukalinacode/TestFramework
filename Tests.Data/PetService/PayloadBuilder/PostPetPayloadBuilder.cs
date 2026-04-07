using Models.PetService.General;
using Models.PetService.Payload;
using System;
using System.Collections.Generic;
using Tests.Data.Attributes;

namespace Tests.Data.PetService.PayloadBuilder
{
    /// <summary>
    /// Builder for constructing PostPetPayload instances.
    /// Provides a flexible way to create valid and invalid test data.
    /// </summary>
    public class PostPetPayloadBuilder
    {
        private long id = GenerateId();

        private Category? category = new()
        {
            Id = GenerateId(),
            Name = "Rabbits"
        };

        private string name = $"qa-pet-{GenerateId()}";

        private List<string> photoUrls = ["https://example.com/default.jpg"];

        private List<Tag>? tags =
        [
            new Tag
            {
                Id = GenerateId(),
                Name = "Domesticated"
            }
        ];

        private string status = "available";

        private static long GenerateId()
        {
            return Random.Shared.NextInt64(1, int.MaxValue);
        }

        public PostPetPayloadBuilder Apply(string field, string value)
        {
            return BddFieldApplier.Apply(this, field, value);
        }

        [BddField("id")]
        public PostPetPayloadBuilder SetId(string value)
        {
            if (!long.TryParse(value, out var parsedId))
                throw new FormatException($"Invalid id value: '{value}'.");

            id = parsedId;
            return this;
        }

        [BddField("name")]
        public PostPetPayloadBuilder SetName(string? value)
        {
            name = value!;
            return this;
        }

        [BddField("category")]
        public PostPetPayloadBuilder SetCategory(string? value)
        {
            category = new Category
            {
                Id = GenerateId()!,
                Name = value!
            };
            return this;
        }

        [BddField("tag")]
        public PostPetPayloadBuilder AddTag(string value)
        {
            tags ??= [];
            tags.Add(new Tag
            {
                Id = GenerateId(),
                Name = value
            });
            return this;
        }

        [BddField("status")]
        public PostPetPayloadBuilder SetStatus(string value)
        {
            status = value;
            return this;
        }

        [BddField("photoUrl")]
        public PostPetPayloadBuilder AddPhotoUrl(string value)
        {
            photoUrls ??= [];
            photoUrls.Add(value);
            return this;
        }

        public PostPetPayload Build()
        {
            return new PostPetPayload
            {
                Id = id,
                Category = category,
                Name = name,
                PhotoUrls = photoUrls,
                Tags = tags,
                Status = status
            };
        }
    }
}