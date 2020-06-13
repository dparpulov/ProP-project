<?php
session_start();
require('classes/Subscribe.php');

if (isset($_POST['FirstName']) && isset($_POST['LastName']) && isset($_POST['Email'])) {
    $FirstName = $_POST['FirstName'];
    $LastName = $_POST['LastName'];
    $Email = $_POST['Email'];

    $Subscribe = new Subscribe();
    $Subscribe->Sub($FirstName, $LastName, $Email);
}
?>

<?php include_once("header.php") ?>
<div id="homeCarousel" class="carousel slide" data-ride="carousel">

    <ol class="carousel-indicators">
        <li data-target="#homeCarousel" data-slide-to="0" class="active"></li>
        <li data-target="#homeCarousel" data-slide-to="1"></li>
        <li data-target="#homeCarousel" data-slide-to="2"></li>
    </ol>
    <div class="container-fluid clock h-100">
        <div class="row justify-content-center align-items-center h-100">
            <div class="col-lg-6 countdown rounded text-center text-white">
                <p class="h2 font-weight-light">ONFest will turn on in ...</p>
                <div id="clock" class="display-1"></div>
            </div>
        </div>
    </div>
    <div class="carousel-inner">
        <div class="carousel-item active">
            <img class="d-block w-100" src="img/header1.jpg">
        </div>
        <div class="carousel-item">
            <img class="d-block w-100" src="img/header2.jpg">
        </div>
        <div class="carousel-item">
            <img class="d-block w-100" src="img/header3.jpg">
        </div>
    </div>
    <a class="carousel-control-prev carousel-priority" href="#homeCarousel" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
    </a>
    <a class="carousel-control-next carousel-priority" href="#homeCarousel" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
    </a>
</div>
<div class="row no-gutters">
    <div class="col-lg-6">
        <div class="jumbotron jumbotron-fluid line-up m-0">
            <div class="container text-center">
                <p class="display-4 font-weight-bold text-uppercase">Line Up</p>
                <p class="lead p-5">We will be joined by some of the most famous DJ's of our time. People like: Don
                    Diablo, Tiesto,
                    French Montana and many many more.</p>
                <a class="btn btn-warning btn-md badge-pill text-uppercase px-4" href="line-up.php" role="button">Learn more</a>
            </div>
        </div>
    </div>
    <div class="col-lg-6">
        <div class="jumbotron jumbotron-fluid festival-camping m-0">
            <div class="container text-center">
                <p class="display-4 font-weight-bold text-uppercase">Festival and Camping</p>
                <p class="lead p-5">While at the festival you will have the opportunity to sleep right next to the
                    epicenter of this
                    once in a lifetime experience. You can reserve a camping spot and use it whenever you want.</p>
                <a class="btn btn-warning btn-md badge-pill text-uppercase px-4" href="information.php" role="button">Learn more</a>
            </div>
        </div>
    </div>
</div>
<div class="row no-gutters">
    <div class="col-lg-6">
        <div class="jumbotron jumbotron-fluid ticket m-0">
            <div class="container text-center">
                <p class="display-4 font-weight-bold text-uppercase">Tickets</p>
                <p class="lead p-5">In order to join this great experience, you will have to register and buy your
                    personal
                    ticket.</p>
                <a class="btn btn-warning btn-md badge-pill text-uppercase px-4" href="<?php echo (isset($_SESSION['user_name']) ? 'tickets.php' : 'login-register.php'); ?>" role="button">Learn more</a>
            </div>
        </div>
    </div>
    <div class="col-lg-6">
        <div class="jumbotron jumbotron-fluid newsletter m-0">
            <div class="container text-center">
                <p class="display-4 font-weight-bold text-uppercase">Subscribe</p>
                <div class="newsletter px-5">
                    <p class="lead">Stay up to date by subscribing to our newsletter!</p>
                    <form action="" method="post">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="input-group mb-3">
                                    <input type="text" name="FirstName" class="form-control badge-pill"
                                           placeholder="First Name">
                                    <input type="text" name="LastName" class="form-control badge-pill"
                                           placeholder="Last Name">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="input-group mb-3">
                                    <input type="text" name="Email" class="form-control badge-pill"
                                           placeholder="Email Address">
                                    <div class="input-group-append">
                                        <button type="submit" name="Subscribe"
                                                class="btn btn-warning btn-md badge-pill text-uppercase px-4">Subscribe</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

<?php include_once 'footer.php' ?>


</body>
</html>