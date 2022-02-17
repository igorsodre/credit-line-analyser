using System;
using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Services;
using FluentAssertions;
using Xunit;

namespace Tests.Unit;

public class StartupAnalyserTests
{
    private readonly StartupAnalyser _sut;

    public StartupAnalyserTests()
    {
        _sut = new StartupAnalyser();
    }

    [Fact]
    public void Analyse_ForValidCreditLineRequestBasedOnMontlyRevenew_ReturnsSccessResult()
    {
        // Arrange
        var request = new CreditLineRequest
        {
            CashBalance = 435.30M,
            FoundingType = "Startup",
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
    public void Analyse_ForValidCreditLineRequestBasedOnCashBalance_ReturnsSccessResult()
    {
        // Arrange
        var request = new CreditLineRequest
        {
            CashBalance = 2541.5M,
            FoundingType = "Startup",
            MonthlyRevenue = 2000M,
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
            FoundingType = "Startup",
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
