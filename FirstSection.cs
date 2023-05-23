/*Add a button with btnPayment ID and create btnPayment_Click server event related to it*/
protected void btnPayment_Click(object sender, EventArgs e)
{
    Dictionary<string, string> pairs = new Dictionary<string, string>
    {
        { "fName", "Reza" },
        { "lName", "Paidar" },
        { "gender", "1" },
        { "mobileNum", "+989198877456" },
        { "refCode" ,"1234" },
        { "pId" ,"1" },
        { "referId", "1234" },
    };
    Session["pairs"] = pairs;

    string merchantId = "********-****-****-****-************"; //Get it from Zarin pal
    //string callback = "http://yourdomain.ir/Client/payment"; 
    string callback = "https://localhost:44382/Client/payment"; // Use this link in debug mobe otherwise use a real url which exist on you host.

    string desc = "Try write a short description of your product";
    int price = 1000; //Zarin pal use Toman uint in this case, 1000 Tomans is good for test, but in real you should set real price related to your product in Toman uint.
    Session["price"] = price.ToString();
    myZarin.PaymentGatewayImplementationServicePortType payment =
        new myZarin.PaymentGatewayImplementationServicePortTypeClient();

    myZarin.PaymentRequestRequestBody requestBody =
        new myZarin.PaymentRequestRequestBody(merchantId, price, desc, "", mobileNum, callback);

    myZarin.PaymentRequestRequest request = new myZarin.PaymentRequestRequest(requestBody);
    var result = payment.PaymentRequest(request);
    if (result.Body.Status > 0)
    {
        Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + result.Body.Authority);
    }
}
