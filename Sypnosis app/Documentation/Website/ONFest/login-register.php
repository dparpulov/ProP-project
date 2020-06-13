<?php
session_start();
require('classes/User.php');
if (isset($_POST['registerSubmit'])) {

    $registerFirstName = $_POST['FirstName'];
    $registerLastName = $_POST['LastName'];
    $registerEmail = $_POST['email'];
    $registerPassword = $_POST['password'];
    $registerCheckPassword = $_POST['confirmPassword'];
    $registerDob = $_POST['dob'];
    $gender = $_POST['gender'];

    $register = new User();

    if ( $registerPassword == $registerCheckPassword) {
        if ($register->UserRegister($registerFirstName, $registerLastName, $registerEmail, $registerPassword)) {
            header("Location: index.php");
        }
    } else {
        echo '<script>alert("Confirmed password different");window.history.go(-1);</script>';
    }
}
if (isset($_POST['login-submit'])) {
    $loginEmail = $_POST['email'];
    $loginPassword = $_POST['password'];

    $login = New User();

    if ($login->UserLogin($loginEmail, $loginPassword)) {
        header("Location: index.php");
    }
}
?>

<?php include_once("header.php") ?>

<div class="jumbotron jumbotron-fluid newsletter">
    <div class="container py-3">
		<h1 class="display-4 font-weight-bold text-uppercase text-center">Login/Register</h1>
    </div>
</div>

<div class="container my-5">
    <div class="row justify-content-center">
        <div class="col-lg-6">
            <div class="panel panel-login">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-lg-6">
                            <a href="#" id="login-form-link" class="active">Login</a>
                        </div>
                        <div class="col-lg-6">
                            <a href="#" id="register-form-link">Register</a>
                        </div>
                    </div>
                    <hr>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <form id="login-form" action="" method="post" role="form">
                                <div class="form-group">
                                    <option>Email</option>
                                    <input type="text" name="email" id="email" tabindex="1" class="form-control"
                                           placeholder="Email" value="" required>
                                </div>
                                <div class="form-group">
                                    <option>Password</option>
                                    <input type="password" name="password" id="password" tabindex="2"
                                           class="form-control" placeholder="Password" required>
                                </div>
                                <div class="form-group text-center">
                                    <input type="checkbox" tabindex="3" class="" name="remember" id="remember">
                                    <label class="font-weight-bold"> Remember Me</label>
                                </div>
                                <div class="form-group">
                                    <div class="row justify-content-center">
                                        <div class="col-lg-6">
                                            <input type="submit" name="login-submit" id="login-submit" tabindex="4"
                                                   class="form-control btn btn-login" value="Log In">
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="text-center">
                                                <a href="https://phpoll.com/recover" tabindex="5"
                                                   class="forgot-password">Forgot Password?</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                            <form id="register-form" action="" method="post"
                                  role="form" style="display: none;">
                                <div class="form-group">
                                    <option>First name</option>
                                    <input type="text" name="FirstName" id="First name" tabindex="1"
                                           class="form-control"
                                           placeholder="First name" value="" required>
                                </div>
                                <div class="form-group">
                                    <option>Last name</option>
                                    <input type="text" name="LastName" id="Last name" tabindex="1" class="form-control"
                                           placeholder="Last name" value="" required>
                                </div>
                                <div class="form-group">
                                    <option>Email</option>
                                    <input type="email" name="email" id="email" tabindex="1" class="form-control"
                                           placeholder="Email Address" value="" required>
                                </div>
                                <div class="form-group">
                                    <option>Password</option>
                                    <input type="password" name="password" id="password" tabindex="2"
                                           class="form-control" placeholder="Password" required>
                                </div>
                                <div class="form-group">
                                    <option>Confirm password</option>
                                    <input type="password" name="confirmPassword" id="confirm-password" tabindex="2"
                                           class="form-control" placeholder="Confirm Password" required>
                                </div>





                                <div class="form-group">
                                    <div class="row justify-content-center">
                                        <div class="col-lg-6">
                                            <input type="submit" name="registerSubmit" id="register-submit"
                                                   tabindex="4" class="form-control btn btn-register"
                                                   value="Register Now">
                                        </div>
                                    </div>
                                </div> <!-- /.form-group -->
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<?php include_once 'footer.php' ?>


</body>
</html>
