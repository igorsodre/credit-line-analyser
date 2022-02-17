using System;
using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Services;
using FluentAssertions;
using Xunit;

namespace Tests.Unit;

public class SmeAnalyserTests
{
    private readonly SmeAnalyser _sut;

    public SmeAnalyserTests()
    {
        _sut = new SmeAnalyser();
    }

    [Fact]
    public void Analyse_ForValidCreditLineRequest_ReturnsSccessResult()
    {
        // Arrange
        var request = new CreditLineRequest
        {
            CashBalance = 435.30M,
            FoundingType = "SME",
            MonthlyRevenue = 4235.45M,
            RequestedCreditLine = 847,
            RequestedDate = DateTime.Now
        };

        // Act
        var result = _sut.Analyse(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public void Analyse_ForInvalidCreditLineRequest_ReturnsFailResult()
    {
        // Arrange
        var request = new CreditLineRequest
        {
            CashBalance = 435.30M,
            FoundingType = "SME",
            MonthlyRevenue = 4235.45M,
            RequestedCreditLine = 848,
            RequestedDate = DateTime.Now
        };

        // Act
        var result = _sut.Analyse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessages.Should().BeEquivalentTo("Credit line requested is too high");
    }
}
