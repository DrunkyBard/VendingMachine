function VendingMachineViewModel() {
    var self = this;
    this.BuyerWallet = ko.observableArray();
    this.MachineWallet = ko.observableArray();
    this.AvailableGoods = ko.observableArray();
    this.DepositCoins = ko.observableArray();
    this.WalletSort = function (l, r) {
        return l.ParValue() > r.ParValue() ? -1 : 1;
    }
    this.UpdateWallets = function (newBuyerWallet, newMachineWallet) {
        self.BuyerWallet.removeAll();
        ko.utils.arrayForEach(newBuyerWallet, function (coin) {
            self.BuyerWallet.push(new Coin(coin.ParValue, coin.Count));
        });
        self.MachineWallet.removeAll();
        ko.utils.arrayForEach(newMachineWallet, function (coin) {
            self.MachineWallet.push(new Coin(coin.ParValue, coin.Count));
        });
        self.DepositCoins.removeAll();
    }

    this.DepositTotal = ko.computed(function () {
        var total = 0;
        ko.utils.arrayForEach(self.DepositCoins(), function (coin) {
            var value = coin.ParValue() * coin.Count();
            total += value;
        });

        return total;
    }, this);

    this.Refund = function () {
        $.post("/Home/Refund", { deposit: ko.mapping.toJS(self.DepositCoins) }, function (coinRefundedEvent) {
            if (coinRefundedEvent.Error) {
                bootbox.alert(coinRefundedEvent.Message);

                return;
            }

            self.UpdateWallets(coinRefundedEvent.BuyerWallet, coinRefundedEvent.VendingMachineWallet);
        });
    }

    this.Deposit = function (coin) {
        self.DepositCoins.push(new Coin(coin.ParValue(), 1));
        coin.Count(coin.Count() - 1);

        if (coin.Count() === 0) {
            self.BuyerWallet.remove(coin);
        }
    }

    this.Buy = function (goods) {
        $.post("/Home/Buy", { deposit: ko.mapping.toJS(self.DepositCoins), goodsIdentity: goods.Identity }, function (goodsBuyedEvent) {
            if (goodsBuyedEvent.Error) {
                bootbox.alert(goodsBuyedEvent.Message, function () {
                    if (goodsBuyedEvent.ErrorType === "VendingMachineDoesNotHaveCoinsForRefundException") {
                        location.reload();
                    }
                });

                return;
            }

            bootbox.alert("Спасибо!");
            self.UpdateWallets(goodsBuyedEvent.UpdatedBuyerWallet, goodsBuyedEvent.UpdatedMachineWallet);
            goods.Count(goods.Count() - 1);
        });
    }
}