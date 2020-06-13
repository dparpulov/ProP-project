<?php
session_start();
require('classes/CampingSpot.php');
$cam = new CampingSpot();
$campspot= $cam->GetCampSpot();
require ('classes/Ticket.php');
 if (isset($_SESSION['user_id'])&&isset($_POST['ticketBuy'])) {
        $accountId = $_SESSION['user_id'];
        $campId = $_POST["campNumber"];
        $purchase = new Ticket();
        $purchase->Purchase($accountId,$campId);
    }
?>
<?php include_once("header.php") ?>

<div class="jumbotron jumbotron-fluid ticket">
    <div class="container py-3">
        <h1 class="display-4 font-weight-bold text-uppercase text-center">Tickets</h1>
    </div>
</div>

<div class="container my-5">
    <div class="text-center text-uppercase">
        <h1>Buy Tickets</h1>
    </div>
    <div class="input-group mb-3 ">
        <div class="input-group-prepend">
            <label class="input-group-text" for="inputGroupSelect01">Number</label>
        </div>
        <select class="custom-select" id="inputGroupSelect01">
            <option selected value="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1 -------------------------------------------------------------------------------------------------------------------------------- 55euro</option>
        </select>
    </div>
    <form action="" method="post" >
    <div class="input-group mb-3 ">
        <div class="input-group-prepend">
            <label class="input-group-text" for="inputGroupSelect01">Camping Spot</label>
        </div>
        <select class="custom-select" id="campingSelect" name="campNumber" onchange="Price()">
            <option class="custom-select" value="Null">Null</option>
        <?php
        if(campspot!=null) {
            foreach ($campspot as $a) {
                echo '
                    <option class="custom-select" value="' . $a["id"] . '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' . $a["id"] . '--------------------------------------------10euro for reservation-------------------------------------------- 20euro</option>
                    ';
            }
        }
        ?>
        </select>
    </div>
<!--price for the label-->
<script>
    function Price() {
        var totalPrice = 55;
        if($("#campingSelect option:selected").text() != 'Null'){
            totalPrice +=  30;
        }
        console.log(totalPrice);
        $('#priceAlert').html(totalPrice);
    }
</script>

    <h1 class="text-center">Price:</h1>
    <div class="alert alert-success" id="priceAlert" role="alert">
        55
    </div>
        <div class="text-center">
            <button class="btn btn-primary btn-lg " type="submit" value="buy" name="ticketBuy">Buy</button>
        </div>
    </form>
</div>

<?php include_once 'footer.php' ?>

</body>
</html>