            <form class="form-horizontal" action="index.php?page=login" method="post">
                    <input type='hidden' name='action' value='login'/>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="login">Login:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="login" name="user[login]" placeholder="Enter login" required>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="pwd">Password:</label>
                        <div class="col-sm-8"> 
                            <input type="password" class="form-control" id="pwd" name="user[pass]" placeholder="Enter password" required>
                        </div>
                    </div>
                    
                    <?php
                    global $action;
                    
                    if($action == "login")
                    {
                        echo "
                            <div class='form-group'>
                                <label class='col-sm-offset-2 col-sm-8' style='color:red'>Wrong username or password!</label>
                            </div>";
                    }
                    ?>
                    
                    <div class="form-group"> 
                        <div class="col-sm-offset-2 col-sm-8">
                            <button type="submit" class="btn btn-default">Login</button>
                            <a href="index.php?page=register" class="btn btn-info" role="button">Register</a>
                        </div>
                    </div>
                </form>