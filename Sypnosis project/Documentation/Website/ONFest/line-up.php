<?php
session_start();
require('classes/Artist.php');
$pic = new Artist();
$pictures = $pic->GetPictures();
?>

<?php include_once("header.php") ?>

<div class="jumbotron jumbotron-fluid line-up">
    <div class="container py-3">
		<h1 class="display-4 font-weight-bold text-uppercase text-center">Line-Up</h1>
    </div>
</div>

<div class="container my-5">
    <div class="row">
        <?php
        if ($pictures != null) {
            foreach ($pictures as $a) {
                echo '
                        <div class="col-lg-4">
                            <div class="card mb-4 box-shadow">
                                <img class="card-img-top" data-src="holder.js/100px225?theme=thumb&bg=55595c&fg=eceeef&text=Thumbnail" 
                                    src="data:image/jpeg;base64,' . base64_encode($a["picture"]) . '" width="100" height="200"/>
                                <div class="card-body">
                                    <p class="card-text">' . $a["name"] . '</p>
                                    <div class="d-flex justify-content-between align-items-center">
                
                                    </div>
                                </div>
                            </div>
                        </div>
                    ';
            }
        }
        ?>
    </div>
</div>

<?php require 'footer.php'; ?>


</body>
</html>