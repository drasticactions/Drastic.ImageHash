// <copyright file="CompareHashTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;
using Sut = Drastic.ImageHash.CompareHash;

namespace Drastic.ImageHash.Test
{
    public class CompareHashTest
    {
        private const int VALIDHASHSIZE = 8;

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void NullArgumentShouldThrowArgumentNullExceptionTest(bool hash1IsNull, bool hash2IsNull)
        {
            // arrange
            var hash1 = hash1IsNull ? null : new byte[VALIDHASHSIZE];
            var hash2 = hash2IsNull ? null : new byte[VALIDHASHSIZE];

            // act
            Action act = () => Sut.Similarity(hash1, hash2);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(9)]
        public void SimilarityThrowsExceptionOnWrongInputSizeArgument(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Similarity(new byte[size], new byte[VALIDHASHSIZE]));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Similarity(new byte[VALIDHASHSIZE], new byte[size]));
        }

        [Fact]
        public void OneBitDifferenceShouldResultInAlmost99PercentSimilarityTest()
        {
            // arrange
            var hash1 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
            var hash2 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 254 };

            // act
            var result = Sut.Similarity(hash1, hash2);

            // assert
            result.Should().Be(98.4375);
        }

        [Fact]
        public void SameHashShouldResultIn100PercentSimilarityTest()
        {
            // arrange
            var hash = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };

            // act
            var result = Sut.Similarity(hash, hash);

            // assert
            result.Should().Be(100);
        }
    }
}
