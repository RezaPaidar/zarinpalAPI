protected void Page_Load(object sender, EventArgs e)
{
    string merchantId = "********-****-****-****-************"; //In any case, always you need authentication with merchantId

    /*After customer pay in zarinpal page, he/she redirect this page as you set callback url in first section,
    you can get 2 important query strings "Authority" and "Status" like this:*/
    var collection = HttpUtility.ParseQueryString(this.ClientQueryString);
    string Authority = collection["Authority"];
    string Status = collection["Status"];

    int price = 0;
    if (Session["price"] != null)
    {
        price = int.Parse(Session["price"].ToString());
    }
    else
    {
        Session["status"] = -999;
        Session["TDate"] = DateTime.Now;
        Response.Redirect("Support");// it's up to you which page to call support
    }    

    myZarin.PaymentGatewayImplementationServicePortType payment = new myZarin.PaymentGatewayImplementationServicePortTypeClient();
    myZarin.PaymentVerificationRequestBody requestBody = new myZarin.PaymentVerificationRequestBody(
        merchantId, Authority, price);
    myZarin.PaymentVerificationRequest request = new myZarin.PaymentVerificationRequest(requestBody);

    var result = payment.PaymentVerification(request);
    //Just in this case you can realise that case was successful :)
    if (result.Body.Status > 0 && Status == "OK")
    {
        Session["status"] = 1;
        Session["ref_id"] = result.Body.RefID;
        Session["TDate"] = DateTime.Now;
        Response.Redirect("paymentResult");

        /*You can all details from previous page by a session dictionary or other way, it's up to you*/
        var pairs = Session["pairs"] as Dictionary<string, string>;
        var uFName = pairs["fName"].ToString();
        var uLName = pairs["lName"].ToString();
        int gender = int.Parse(pairs["gender"]);
        var PhoneNumber = pairs["mobileNum"].ToString();
        var refCode = pairs["refCode"].ToString();
        var pId = int.Parse(pairs["pId"].ToString());
        var RefrenceNumber = pairs["referId"].ToString();
        var SaleRefrenceId = int.Parse(Session["ref_id"].ToString());
        var StatusPayment = int.Parse(Session["status"].ToString());

        /*You can add details to you DB after successful transaction like this:*/
        /*------------------------------------------------------------------------------------------*/
        _user.AddUserRegisterByMobile(uFName, uLName, gender,PhoneNumber, refCode, pId, RefrenceNumber
            , SaleRefrenceId, StatusPayment, price.ToString(), "zarinpal");
        /*------------------------------------------------------------------------------------------*/
    }
    else // case wasn't successful :(
    {
        Session["status"] = result.Body.Status;
        Session["TDate"] = DateTime.Now;
        Response.Redirect("paymentResult");

    }
}
