<!DOCTYPE html>
<html lang="en">
<head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="stylesheet" href="css/stylesheet.css">
    <script src="http://ajax.googlepis.com/ajax/libs/jquery/2.20/jquery.min.js"></script>
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"
          integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.13/css/all.css"
          integrity="sha384-DNOHZ68U8hZfKXOrtjWvjxusGo9WQnrNx2sqG0tfsghAvtVlRW3tvkXWZh58N9jp" crossorigin="anonymous">
    <title>ONFest - <?php
        switch (basename($_SERVER['PHP_SELF']))
        {
            case 'index.php':
                echo "Home";
                break;
            case 'line-up.php':
                echo "Line-Up";
                break;
            case 'information.php':
                echo "Information";
                break;
            case 'tickets.php':
                echo "Tickets";
                break;
            case 'login-register.php':
                echo "Login/Register";
                break;
            case 'account.php':
                echo $_SESSION['user_name'];
                break;
        }?>
    </title>
</head>
<body>
<nav class="navbar fixed-top navbar-expand-lg navbar-dark py-3">
    <a class="navbar-brand" href="index.php">
        <img src="img/onFESTlogo.png" width="auto" height="50">
    </a>
    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup"
            aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
        <ul class="navbar-nav ml-auto lead text-uppercase">
            <li class="nav-item <?php if(basename($_SERVER['PHP_SELF']) == 'index.php') { echo 'active'; } ?>">
                <a class="nav-link" href="index.php">Home <span class="sr-only">(current)</span></a>
            </li>
            <li class="nav-item <?php if(basename($_SERVER['PHP_SELF']) == 'line-up.php') { echo 'active'; } ?>">
                <a class="nav-link" href="line-up.php">Line-Up</a>
            </li>
            <li class="nav-item <?php if(basename($_SERVER['PHP_SELF']) == 'information.php') { echo 'active'; } ?>">
                <a class="nav-link" href="information.php">Information</a>
            </li>
            <?php if(isset($_SESSION['user_name'])) {  ?>
				<li class="nav-item <?php if(basename($_SERVER['PHP_SELF']) == 'tickets.php') { echo 'active'; } ?>">
					<a class="nav-link" href="tickets.php">Tickets</a>
				</li>					
				<li class="nav-item dropdown <?php if(basename($_SERVER['PHP_SELF']) == 'account.php') { echo 'active'; } ?>">
					<a class="nav-link dropdown-toggle text-capitalize" href="#" id="accountDropdown" data-toggle="dropdown"
					   aria-haspopup="true" aria-expanded="false"><?php echo $_SESSION['user_name'];?></a>
					<div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownMenuLink">
						<a class="dropdown-item" href="account.php"><i class="fas fa-user"></i> Account</a>
						<div class="dropdown-divider"></div>
						<a class="dropdown-item" name="logout" href="logout.php"><i class="fas fa-sign-out-alt"></i> Logout</a>
					</div>
				</li>
            <?php }
            else{
            ?>
				<li class="nav-item <?php if(basename($_SERVER['PHP_SELF']) == 'login-register.php') { echo 'active'; } ?>">
					<a class="nav-link" href="login-register.php">Login/Register</a>
				</li>
            <?php } ?>
        </ul>
    </div>
</nav>