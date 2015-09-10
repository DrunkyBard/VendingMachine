using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.Commands;
using VendingMachineApp.DataAccess.Queries;
using VendingMachineApp.Models;

namespace VendingMachineApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RefundCommandHandler _refundCommandHandler;
        private readonly BuyCommandHandler _buyCommandHandler;

        public HomeController(RefundCommandHandler refundCommandHandler, BuyCommandHandler buyCommandHandler)
        {
            Contract.Requires(refundCommandHandler != null);
            Contract.Requires(buyCommandHandler != null);

            _refundCommandHandler = refundCommandHandler;
            _buyCommandHandler = buyCommandHandler;
        }

        public ActionResult Index()
        {
            var query = new ShowVendingMachineQuery();
            var model = query.Ask();

            return View(model);
        }

        public JsonResult Refund(IEnumerable<CoinViewModel> deposit)
        {
            var depositCoins = deposit
                .Select(x => new Coin(x.ParValue, x.Count))
                .ToArray();
            var command = new RefundCommand(depositCoins);

            var @event = _refundCommandHandler.Execute(command);

            return Json(@event);
        }

        public void Buy(IEnumerable<CoinViewModel> deposit, Guid goodsIdentity)
        {
            var depositCoins = deposit
                .Select(x => new Coin(x.ParValue, x.Count))
                .ToArray();
            var command = new BuyCommand(depositCoins, new GoodsIdentity(goodsIdentity));
            var goodsBuyed = _buyCommandHandler.Execute(command);
        }
    }
}