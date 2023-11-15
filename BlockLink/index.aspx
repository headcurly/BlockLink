<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="BlockLink.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title id="title" runat="server"></title>
    <script src="js/jquery-1.12.4.js"></script>
    <style>
        html, body {
            height: 100%;
            width: 100%;
            padding: 0;
            margin: 0;
        }

        #blockLink {
            width: 80%;
            float: left;
            min-height: 20px;
            box-sizing: border-box;
            padding: 20px;
            background-color: #fffbdb;
            height: 100%;
            overflow: auto;
        }

        #tool {
            width: 20%;
            float: right;
            padding: 20px;
            box-sizing: border-box;
            background-color: #f8feff;
        }

            #tool div {
                width: 80%;
                margin: 10px;
                border: 1px solid gray;
                cursor: pointer;
                margin-left: auto;
                margin-right: auto;
            }

        .block {
            width: 300px;
            background-color: #8BC34A;
            float: left;
            margin-right: 10px;
            margin-bottom: 10px;
            min-height: 150px;
        }

            .block i {
                padding: 3px;
                display: block;
                font-size: 10px;
            }

        .blockhead {
            width: 100%;
            text-align: center;
            background-color: #4CAF50;
            line-height: 30px;
        }

        .time {
        }

        .data {
            word-wrap: break-word;
        }

        .hash {
            word-wrap: break-word;
            color: #9C27B0;
        }

        .prehash {
            word-wrap: break-word;
            color: #3F51B5;
        }

        .line {
            border-top: 1px dashed #FF9800 !important;
            border-bottom: none !important;
            width: 100% !important;
            margin: 20px 0 20px 0;
        }

        #getLink {
            background-color: #8BC34A;
            height: 30px;
            text-align: center;
            line-height: 30px;
        }

        #CreatLink {
            background-color: #00BCD4;
            height: 30px;
            text-align: center;
            line-height: 30px;
        }

        #form1 {
            height: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="blockLink">
        </div>
        <div id="tool">
            <h3>从用户获取生成主链：</h3>
            <div id="getLink">获取区块链</div>
            <br />
            <h3>用户添加内容区块：</h3>
            <h4>用户名：</h4>
            <input id="BlockUser" style="width: 100%;" />
            <h4>数据：</h4>
            <textarea id="BlockData" style="width: 100%; height: 200px;"></textarea>
            <div id="CreatLink">创建块区块链</div>
        </div>
        <div class="temple" style="display: none;">
            <div class="blocktemp">
                <div class="blockhead">编号</div>
                <i>
                    <label>时间截：</label><span class="time"></span></i>
                <i>
                    <label>数据：</label><span class="data"></span></i>
                <i>
                    <label>HASH:</label><span class="hash"></span></i>
                <i>
                    <label>PREHASH:</label><span class="prehash"></span></i>
            </div>
        </div>
    </form>
    <script>
        $(function () {
            $("#getLink").click(function () {
                $.ajax({
                    type: "POST",
                    datatype: "xml",
                    url: "/Service.asmx/GetBlockLink",
                    success: function (result) {
                        blockview(result);
                    }
                });
            });

            //创建
            $("#CreatLink").click(function () {
                var data = $("#BlockData").val();
                if (data) {
                    $.ajax({
                        type: "POST",
                        datatype: "xml",
                        url: "/Service.asmx/CreatBlock",
                        data: { data: data, user: $("#BlockUser").val() },
                        success: function (result) {
                            blockview(result);
                        }
                    });
                }
            });
        });

        function blockview(result) {
            var data = [];
            data = eval(result);
            var body = $("#blockLink");
            body.html("");
            data.forEach(function (item, index) {
                var div = $(".blocktemp").clone();
                div.removeClass("blocktemp");
                div.addClass("block");
                div.addClass("block" + item.Index);
                div.children(".blockhead").html(item.Index);
                div.children("i").children('.time').html(item.Timestamp);
                div.children("i").children('.data').html(item.Data);
                div.children("i").children('.hash').html(item.Hash);
                div.children("i").children('.prehash').html(item.PrevHash);

                body.append(div);
            });
        }
    </script>
</body>
</html>
