// <copyright file="AverageHashTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Drastic.ImageHash.HashAlgorithms;
using Drastic.ImageHash.Test.Data;
using EasyTestFile;
using EasyTestFileXunit;
using FluentAssertions;
using Xunit;

namespace Drastic.ImageHash.Test.Algorithms
{
    [UsesEasyTestFile]
    public class AverageHashTest
    {
        private readonly AverageHash sut;

        private readonly Dictionary<string, ulong> expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 16701559372701825768 },
            { "Alyson_Hannigan_500x500_1.jpg", 16701559372735380200 },
            { "Alyson_Hannigan_200x200_0.jpg", 16701559372701825768 },
            { "Alyson_Hannigan_4x4_0.jpg", 14395694381845246192 },
            { "github_1.jpg", 15835643108028573695 },
            { "github_2.jpg", 15835645411202688999 },
        };

        public AverageHashTest()
        {
            this.sut = new AverageHash();
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

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 16708314879819185914)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 17284780030169120507)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 17284779961181208314)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 14974406947552165119)]
        [InlineData("github_1.jpg", 18429789604005013503)]
        [InlineData("github_2.jpg", 18429789449788844031)]
        public async Task HashImagesTest3(string filename, ulong expectedHash)
        {
            // arrange
            ulong result;

            // act
            using (Stream stream = await TestData.GetByName(filename).AsStream())
            {
                result = this.sut.Hash(stream);
            }

            // assert
            result.Should().Be(expectedHash);
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
        public void ImageWithFilterShouldHaveAlmostOrExactly100Similarity1Test()
        {
            // arrange
            var hash1 = this.expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = this.expectedHashes["Alyson_Hannigan_500x500_1.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(98.4375);
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
        public void ComparingExtremelySmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = this.expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = this.expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(82.8125);
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
            result.Should().Be(89.0625);
        }
    }
}
