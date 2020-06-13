<?php
session_start();
include('classes/User.php');
include('classes/Ticket.php');
require ('classes/Invite.php');
$Update=new User();
$Update->Update($_SESSION['email']);
$Update->Update($_SESSION['user_id']);
$userAccount = new User();
$ticket = new Ticket();
$invite = new Invite();

$picture=$userAccount->GetPictures($_SESSION['email']);

if (isset($_POST['changePassword'])){
    $ChangPassword=new User();
    if ($ChangPassword->ChangePassword($_POST['oldPassword'],$_POST['newPassword'],$_SESSION['email'])){
        header("Location:logout.php");
    }
}
if (isset($_POST['btnDeposit'])){
    $Deposit=new User();
    if ($Deposit->Deposit($_POST['Amount'], $_SESSION['email'], $_SESSION['balance'])){
        header("Location:account.php");
    }
}
if (isset($_POST['btnWithdraw'])){
    $Withdraw=new User();
    if ($Withdraw->Withdraw($_POST['Amount'], $_SESSION['email'], $_SESSION['balance'])){
        header("Location:account.php");
    }
}
if(isset($_POST['Invite'])){
    $accountId = $_SESSION['user_id'];
    $email = $_POST['email'];
    $invite->InvitePeople($email,$accountId);
}
if(isset($_POST['Reserve'])){
    $campId = $_POST["campNumber"];
    $ticket->ReserveCampingSpot($campId,$_SESSION['user_id']);
}
if(isset($_POST['insert'])){
    $file = file_get_contents($_FILES["image"]["tmp_name"]);
    $userAccount->UploadPicture($file,$_SESSION['email']);
}
?>

<?php include_once("header.php") ?>
<div class="jumbotron jumbotron-fluid newsletter">
    <div class="container py-3">
        <h1 class="display-4 font-weight-bold text-uppercase text-center"><?php echo $_SESSION['user_name']; ?></h1>
    </div>
</div>
<div class="container my-5">
    <div class="row">
        <div class="col-lg-3 text-center px-1">
            <div class="card mx-auto" style="width: 18rem;">
                <?php
                if($picture!=null){
					echo'<div><img class="card-img-top" data-src="holder.js/100px225?theme=thumb&bg=55595c&fg=eceeef&text=Thumbnail" 
                                    src="data:image/jpeg;base64,' . base64_encode($picture["picture"]) . '" width="100" height="280"/></div>';
                }else {
                    echo'<div><img class="card-img-top" src="img/no-profile.jpg" ></div>';
                }
                ?>
<!--                <img class="card-img-top" src="img/no-profile.jpg" >-->
                <ul class="list-group list-group-flush">
                    <button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-qr-code">Show QR-Code</button>
                    <button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-password">Edit Profile</button>
                    <button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-balance">Deposit/Withdraw Money</button>
                    <button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-camping">Invite</button>
                    <button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-transaction-history">Show Transaction History</button>
                    <?php echo ($invite->GetCampspotId($_SESSION['user_id']) ? '' : '<button type="button" class="list-group-item list-group-item-action bg-dark text-white" data-toggle="modal" data-target=".modal-reserve">Reserve Camping Spot</button>'); ?>
                </ul>
            </div>
        </div>
        <div class="col-lg-9">
            <table class="table table-hover float-lg-right">
                <thead class="thead-light">
                <tr>
                    <th colspan="3">Account Information</th>
                </tr>
                </thead>
                <tbody>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Email:</th>
                    <td class="col-sm-6"><?php echo $_SESSION['email']; ?></td>
                </tr>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Balance:</th>
                    <td class="col-sm-6">&euro;<?php echo $_SESSION['balance']; ?></td>
                </tr>
                </tbody>
            </table>
            <table class="table table-hover">
                <thead class="thead-light">
                <tr>
                    <th colspan="3">Ticket Information</th>
                </tr>
                </thead>
                <tbody>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Ticket Number:</th>
                    <td class="col-sm-6"><?php echo $ticket->GetTicketIdByUserId($_SESSION['user_id'])
                        ?></td>
                </tr>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Name:</th>
                    <td class="col-sm-6"><?php echo $_SESSION['user_name']; ?></td>
                </tr>
                </tbody>
            </table>
            <table class="table table-hover">
                <thead class="thead-light">
                <tr>
                    <th colspan="3">Camping Information</th>
                </tr>
                </thead>
                <tbody>
                <tr class="d-flex">
                    <th class="col-sm-3" scope="row">Camping Spot:</th>
                    <th class="col-sm-3" scope="row"></th>
                    <td class="col-sm-2"><?php echo $invite->GetCampspotId($_SESSION['user_id']) ?></td>

                </tr>
                </tbody>
            </table>
            <table class="table table-hover">
                <thead class="thead-light">
                <tr>
                    <th colspan="3">Billing Information</th>
                </tr>
                </thead>
                <tbody>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Total Ticket Price:</th>
                    <td class="col-sm-6">&euro;55,00</td>
                </tr>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Total Campsite Price:</th>
                    <td class="col-sm-6">&euro;65,00</td>
                </tr>
                <tr class="d-flex">
                    <th class="col-sm-6" scope="row">Total Price:</th>
                    <td class="col-sm-6">&euro;120,00</td>
                </tr>
                </tbody>
            </table>
        </div>
    </div>
    <!-- QR-Code modal -->
    <div class="modal fade modal-qr-code" tabindex="-1" role="dialog"
         aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalTitleQRCode">QR-Code</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <img src="https://chart.googleapis.com/chart?chs=450x450&cht=qr&chl=<?php echo $ticket->GetTicketIdByUserId($_SESSION['user_id']); ?>&choe=UTF-8"/>
                </div>
                <div class="modal-footer">
                    <p class="lead">
                        &copy; Sypnosis.
                    </p>
                </div>
            </div>
        </div>
    </div>
    <!-- Change Password modal -->
    <div class="modal fade modal-password" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel"
         aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalTitlePassword">Change Profile</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <form name="UploadPicture" id="UploadPicture" action="" method="post" enctype="multipart/form-data">
                    <div class="modal-body">
                        <div class="input-group mb-3">
                            <input type="file" name="image" id="image" class="form-control" placeholder="Upload Picture">
                        </div>
                        <div class="input-group mb-3">
                            <button type="submit" name="insert" id="insert" value="Insert"class="btn btn-primary btn-lg btn-block"> Upload Picture
                            </button>
                        </div>
                    </div>
                </form>

                <script>
                    $(document).ready(function () {
                        $('#insert').click(function)(){
                            var image_name=$('#image').val();
                            if(image_name=='')
                            {
                                alert("Please Select Image");
                                return false;
                            }
                            else{
                                var extension = $('image').val().split('.').pop().toLowerCase();
                                if(jQuery.inArray(extension,['png','jpg','jpeg'])==-1){
                                    alert('Invalid Image File');
                                    $('image').val('');
                                    return false;
                                }
                            }
                        }
                    })
                </script>

                <form name="ChangePassword" id="ChangePassword" action="" method="post">
                    <div class="modal-body">
                        <div class="input-group mb-3">
                            <input type="text" name="oldPassword" class="form-control" placeholder="Enter password">
                        </div>
                        <div class="input-group mb-3">
                            <input type="text" name="newPassword" class="form-control" placeholder="Enter new password">
                        </div>
                        <div class="input-group mb-3">
                            <input type="text" name="checkPassword" class="form-control" placeholder="Re-enter new password">
                        </div>
                        <div class="input-group mb-3">
                            <button type="submit" name="changePassword" class="btn btn-primary btn-lg btn-block">Change Password
                            </button>
                        </div>
                    </div>
                </form>

                <div class="modal-footer">
                    <p class="lead">
                        &copy; Sypnosis.
                    </p>
                </div>
            </div>
        </div>
    </div>
    <!-- Account Balance modal -->
    <div class="modal fade modal-balance" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel"
         aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalBalance">Account Balance</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form name="ChangBalance" method="post" action="" id="ChangBalance">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text badge-pill">&euro;</span>
                            </div>

                            <input type="text" name="Amount" class="form-control" aria-label="Amount">
                            <div class="input-group-append">
                                <button name="btnDeposit" class="btn btn-outline-secondary" type="submit" id="btnDeposit">
                                    Deposit
                                </button>
                                <button name="btnWithdraw" class="btn btn-outline-secondary badge-pill" type="submit" id="btnWithdraw">
                                    Withdraw
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <p class="lead">
                        &copy; Sypnosis.
                    </p>
                </div>
            </div>
        </div>
    </div>
	<!-- Camping Spot modal -->
	<div class="modal fade modal-camping" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="modalTitlePassword">Invite people to your camping spot</h5>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<form name="Invite" id="Invite" action="" method="post">
					<div class="modal-body">
						<div class="form-group">
                            <option>Email</option>
                            <input type="text" name="email" id="email" tabindex="1" class="form-control"
                                   placeholder="Email" value="" required>
						</div>
						<div class="form-group">
							<button type="submit" name="Invite"
									class="btn btn-primary btn-lg btn-block" <?php echo (!$invite->CheckCampspot($_SESSION['user_id']) ? 'disabled' : ''); ?>>Invite
							</button>
						</div>

						<!--<div class="card-deck">
							<div class="card">
								<img class="card-img-top" src="img/dj.jpg" alt="Card image cap">
								<div class="card-body">
									<h5 class="card-title">Phat Tran</h5>
								</div>
							</div>
							<div class="card">
								<img class="card-img-top" src="img/onFESTlogo.png" alt="Card image cap">
								<div class="card-body">
									<h5 class="card-title">Dongdong Ke</h5>
								</div>
							</div>
							<div class="card">
								<img class="card-img-top" src="img/onFESTlogo.png" alt="Card image cap">
								<div class="card-body">
									<h5 class="card-title">Diqin Yang</h5>
								</div>
							</div>
						</div>-->
					</div>
				</form>
				<div class="modal-footer">
					<p class="lead">
						&copy; Sypnosis.
					</p>
				</div>
			</div>
		</div>
	</div>
    <!-- Transaction History modal -->
    <div class="modal fade modal-transaction-history" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel"
         aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalTransactionHistory">Transaction History</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table class="table table-sm table-striped table-hover">
                        <thead class="thead-dark">
                        <tr>
                            <th>#</th>
                            <th>Name</th>
                            <th>Date Time</th>
                            <th>Quantity</th>
                            <th>Amount</th>
                        </tr>
                        </thead>
                        <tbody>
                        <?php if($userAccount->GetTransactionHistory($_SESSION['user_id']) != null) : ?>
                            <?php $count = 0; ?>
                            <?php foreach ($userAccount->GetTransactionHistory($_SESSION['user_id']) as $tItem): ?>
                                <?php $count++; ?>
                                <tr>
                                    <th scope="row"><?php echo $count; ?></th>
                                    <th><?php echo $tItem['name']; ?></th>
                                    <td><?php echo $tItem['date_time']; ?></td>
                                    <td><?php echo $tItem['quantity']; ?></td>
                                    <td>&euro;<?php echo $tItem['price']; ?></td>
                                </tr>
                            <?php endforeach; ?>
                        <?php endif; ?>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <p class="lead">
                        &copy; Sypnosis.
                    </p>
                </div>
            </div>
        </div>
    </div>
    <!--Reserve a camping spot-->
    <div class="modal fade modal-reserve" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalTitlePassword">Reserve your camping spot</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <form name="Reserve" id="Reserve" action="" method="post">
                    <div class="modal-body">
                        <div class="form-group">
                            <select class="custom-select" id="campingSelect" name="campNumber" onchange="Price()">
                                <option class="custom-select" value="Null">Null</option>
                                <?php require('classes/CampingSpot.php');$cam = new CampingSpot();
                                 $campspot= $cam->GetCampSpot();
                                if($campspot!=null) {
                                    foreach ($campspot as $a) {
                                        echo '<option class="custom-select" value="' . $a["id"] . '">' . $a["id"] . '</option>';
                                    }
                                }
                                ?>
                            </select>
                        </div>
                        <div class="form-group">
                            <button type="submit" name="Reserve"
                                    class="btn btn-primary btn-lg btn-block" >Reserve
                            </button>
                        </div>

                    </div>
                </form>
                <div class="modal-footer">
                    <p class="lead">
                        &copy; Sypnosis.
                    </p>
                </div>
            </div>
        </div>
    </div>
</div>
<?php include_once 'footer.php' ?>
<!-- jQuery first, then Popper.js, then Bootstrap JS -->

</body>
</html>