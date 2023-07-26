using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tema2
{
	public class Program
	{
		public static void Main()
		{
			var invoice = new Invoice(1, DateTime.Now, "John Doe", 6.5m);
			invoice.AddItem("Item 1", 2, 10.00m);
			invoice.AddItem("Item 2", 1, 20.00m);
			invoice.AddItem("Item 3", 3, 30.00m);
			Console.WriteLine(invoice);
		}
	}

	public class Invoice
	{
		public int InvoiceNumber { get; set; }
		public DateTime InvoiceDate { get; set; }
		public string CustomerName { get; set; }
		public string VendorName { get; set; }
		public List<InvoiceItem> Items { get; set; }
		public decimal TaxRate { get; set; }

		public Invoice() { }

		public Invoice(int invoiceNumber, DateTime invoiceDate, string customerName, decimal taxRate, string vendorName = "My Company")
		{
			InvoiceNumber = invoiceNumber;
			InvoiceDate = invoiceDate;
			CustomerName = customerName;
			VendorName = vendorName;
			TaxRate = taxRate;
			Items = new List<InvoiceItem>();
		}

		public void AddItem(string itemName, int quantity, decimal pricePerUnit)
		{
			var item = new InvoiceItem(itemName, quantity, pricePerUnit);
			Items.Add(item);
		}

		public CultureInfo GetSystemCulture()
		{
			Console.WriteLine("For which country do you need this invoice?");
			Console.WriteLine("Examples: for US, type en-US; for Romania, type ro-RO; default option will be the system region.");
			string culture = Console.ReadLine();

			if (string.IsNullOrEmpty(culture))
			{
				return CultureInfo.CurrentCulture;
			}

			try
			{
				return new CultureInfo(culture);
			}
			catch (CultureNotFoundException)
			{
				Console.WriteLine("Invalid culture. Using the system's current culture.");
				return CultureInfo.CurrentCulture;
			}
		}

		public override string ToString()
		{
			CultureInfo culture = GetSystemCulture();

			string itemsList = string.Join("\n", Items);
			decimal subtotal = 0;
			foreach (var item in Items)
			{
				subtotal += item.TotalPrice;
			}
			decimal taxAmount = subtotal * (TaxRate / 100);
			decimal totalAmount = subtotal + taxAmount;

			return $"Invoice Number: {InvoiceNumber}\nInvoice Date: {InvoiceDate.ToString("F", culture)}\nCustomer Name: {CustomerName}\nVendor Name: {VendorName}\nItems:\n{itemsList}\nSubtotal: {subtotal.ToString("C", culture)}\nTax Rate: {TaxRate}%\nTax Amount: {taxAmount.ToString("C", culture)}\nTotal Amount: {totalAmount.ToString("C", culture)}";
		}
	}

	public class InvoiceItem
	{
		public string ItemName { get; set; }
		public int Quantity { get; set; }
		public decimal PricePerUnit { get; set; }
		public decimal TotalPrice { get { return Quantity * PricePerUnit; } }


		public override string ToString()
		{
			//var culture = new Invoice().GetSystemCulture();
			//return $"{ItemName} x {Quantity} = {TotalPrice.ToString("C", culture)}";
			return $"{ItemName} x {Quantity} = {TotalPrice}";
		}

		public InvoiceItem(string itemName, int quantity, decimal pricePerUnit)
		{
			ItemName = itemName;
			Quantity = quantity;
			PricePerUnit = pricePerUnit;
		}
	}
}
