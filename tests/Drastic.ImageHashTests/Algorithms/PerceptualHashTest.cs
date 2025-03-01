// <copyright file="PerceptualHashTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Drastic.ImageHash.HashAlgorithms;
using Drastic.ImageHash.Test.Data;
using EasyTestFileXunit;
using FluentAssertions;
using Xunit;

namespace Drastic.ImageHash.Test.Algorithms
{
    [UsesEasyTestFile]
    public class PerceptualHashTest
    {
        private readonly PerceptualHash sut;

        private readonly Dictionary<string, ulong> expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 17839858461443178030 },
            { "Alyson_Hannigan_500x500_1.jpg", 17839823311430827566 },
            { "Alyson_Hannigan_200x200_0.jpg", 17839858461443178030 },
            { "Alyson_Hannigan_4x4_0.jpg", 17409736169497899465 },
            { "github_1.jpg", 13719320793338945348 },
            { "github_2.jpg", 13783795072850083657 },
        };

        public PerceptualHashTest()
        {
            this.sut = new PerceptualHash();
        }

        [Fact]
        [SuppressMessage("ReSharper", "AccessToDisposedClosure", Justification = "Manually reviewed")]
        public async Task NotAnImageShouldThrowExceptionTest()
        {
            // arrange

            // act
            using Stream stream = await TestData.NotAnImage.AsStream();
            Action act = () => this.sut.Hash(stream);

            // assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void NullArgumentShouldThrowArgumentNullExceptionTest()
        {
            // arrange

            // act
            Action act = () => this.sut.Hash(null);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ImageWithFilterShouldHaveAlmostOrExactly100Similarity1Test()
        {
            // arrange
            var hash1 = this.expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = this.expectedHashes["Alyson_Hannigan_500x500_1.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(96.875);
        }

        [Fact]
        public void ResizedImageShouldHaveAlmostOrExactly100Similarity2Test()
        {
            // arrange
            var hash1 = this.expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = this.expectedHashes["Alyson_Hannigan_200x200_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(100);
        }

        [Fact]
        public void ComparingSmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = this.expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = this.expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(59.375);
        }

        [Fact]
        public void TwoDifferentImagesOfGithubArePrettySimilarTests()
        {
            // arrange
            var hash1 = this.expectedHashes["github_1.jpg"];
            var hash2 = this.expectedHashes["github_2.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(71.875);
        }
    }
}
