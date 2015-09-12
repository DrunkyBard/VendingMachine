function Coin(parValue, count) {
    this.ParValue = ko.observable(parValue);
    this.Count = ko.observable(count);
}

function CoinDto(parValue, count) {
    this.ParValue = parValue;
    this.Count = count;
}