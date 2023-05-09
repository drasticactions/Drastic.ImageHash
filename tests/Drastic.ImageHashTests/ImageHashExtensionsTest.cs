// <copyright file="ImageHashExtensionsTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Drastic.ImageHash.Test.Data;
using EasyTestFileXunit;
using FakeItEasy;
using FluentAssertions;
using SkiaSharp;
using Xunit;
using Sut = Drastic.ImageHash.ImageHashExtensions;

namespace Drastic.ImageHash.Test
{
    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Testing static extension method class")]
    [UsesEasyTestFile]
    public class ImageHashExtensionsTest
    {
        private readonly IImageHash hashAlgorithm;

        public ImageHashExtensionsTest()
        {
            this.hashAlgorithm = A.Fake<IImageHash>();
        }

        [Fact]
        public async Task HashStreamShouldReadStreamAsImageAndPassDataToHashAlgorithmTest()
        {
            // arrange
            A.CallTo(() => this.hashAlgorithm.Hash(A<SKBitmap>._)).Returns(0UL);
            using Stream stream = await TestData.AlysonHannigan200x200_0.AsStream();

            // act
            var result = Sut.Hash(this.hashAlgorithm, stream);

            // assert
            A.CallTo(() => this.hashAlgorithm.Hash(A<SKBitmap>._)).MustHaveHappenedOnceExactly();
            result.Should().Be(0UL);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void NullArgumentShouldThrowArgumentNullExceptionTest(bool hashImplIsNull, bool streamIsNull)
        {
            // arrange
            IImageHash imageHashImplementation = hashImplIsNull ? null : A.Dummy<IImageHash>();
            Stream stream = streamIsNull ? null : new MemoryStream();

            // act
            Action act = () => Sut.Hash(imageHashImplementation!, stream!);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
