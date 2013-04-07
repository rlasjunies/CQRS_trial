<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="AccountManager.WebServices.test" %>

<!DOCTYPE html>
<html>
<head>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/res/js/test.js"></script>
  <title>Calling page methods with jQuery</title>
  <style type="text/css">
    #Result {
      cursor: pointer;
    }
  </style>
</head>
<body>
  <div id="Result">Click here for the time.</div>
    <script>
        $('#Result').click(call);
  </script>
</body>
</html>