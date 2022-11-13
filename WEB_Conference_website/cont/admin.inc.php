<?php
        
$action = @$_REQUEST["action"]."";
$right = @$_SESSION["conference_system"]["right"];
$articleID = @$_REQUEST["articleID"]."";
$userID = @$_REQUEST["userID"]."";
$scoreID = @$_REQUEST["scoreID"];
$set = @$_REQUEST["set"];


$db = new db_articles();
$db->Connect();          

if($action == "delete_article" && $right == "admin")
{
    $db->DeleteArticle($articleID);
}
            
if($action == "approve" && $right == "admin")
{
    $db->ApproveArticle($articleID);
}
            
if($action == "delete_user" && $right == "admin")
{
    $db->DeleteUser($userID);
}
            
if($action == "right" && $right == "admin")
{
    $db->SetRight($userID, $set);
}
            
if($action == "rev" && $right == "admin")
{
    $db->SetReviewer($scoreID["score1"], $set["score1"]);
    $db->SetReviewer($scoreID["score2"], $set["score2"]);
    $db->SetReviewer($scoreID["score3"], $set["score3"]);
}
            
            
?>
            
            <table class="table table-hover ">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Author</th>
                        <th>Link</th>
                        <th>Reviewers</th>
                        <th>Score</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    
                    
<?php
            
$articles = $db->GetAllArticles();
                    
foreach($articles as $art)
{
    $user = $db->GetUser($art["author"]);
    
    $rev1 = $db->GetScore($art["score1"]);
    $rev1 = $db->GetUser($rev1["rev"]);
    $rev2 = $db->GetScore($art["score2"]);
    $rev2 = $db->GetUser($rev2["rev"]);
    $rev3 = $db->GetScore($art["score3"]);
    $rev3 = $db->GetUser($rev3["rev"]);
    
    $score = $db->GetOverallScore($art["score1"], $art["score2"], $art["score3"]);
    $reviewers = $db->GetReviewers();
    
    echo "<tr><td>$art[title]</td>
        <td>$user[login]</td>
        <td><a href='pdf/$art[URL]'>$art[URL]</a></td>
        <td>$rev1[login], $rev2[login], $rev3[login]</td>
        <td>$score</td>
        <form name='form' action='index.php?page=admin&action=rev&articleID=$art[ID]' method='post'>
            <input type='hidden' name='scoreID[score1]' value='$art[score1]'/>
            <input type='hidden' name='scoreID[score2]' value='$art[score2]'/>
            <input type='hidden' name='scoreID[score3]' value='$art[score3]'/>
        <td><select class='form-control' name='set[score1]'>";
    
    foreach($reviewers as $rev)
    {
        echo "<option value='$rev[ID]'>$rev[login]</option>";
    }
    
    echo "</select><select class='form-control' name='set[score2]'>";
    
    foreach($reviewers as $rev)
    {
        echo "<option value='$rev[ID]'>$rev[login]</option>";
    }
    
    echo "</select><select class='form-control' name='set[score3]'>";
    
    foreach($reviewers as $rev)
    {
        echo "<option value='$rev[ID]'>$rev[login]</option>";
    }
    
    echo "</select></td><td>";
    if($art["pass"] == 0) echo "<a href='index.php?page=admin&action=approve&articleID=$art[ID]' class='btn btn-link glyphicon glyphicon-ok' style='color:green'></a>";
    
    echo "<button class='btn btn-link glyphicon glyphicon-refresh' style='color:blue'></button>
            <a href='index.php?page=admin&action=delete_article&articleID=$art[ID]' class='btn btn-link glyphicon glyphicon-remove' style='color:red'></a></td>
        </form></tr>";
}            
            
?> 
            </tbody>
            </table>
                    
            <hr>
                    
            <table class="table table-hover ">
                <thead>
                    <tr>
                        <th>User</th>
                        <th>Right</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
            
<?php
           
$users = $db->GetAllUsers();
                    
foreach($users as $user)
{
    $right = $db->GetRight($user["right"]);
    
    echo "<tr><td>$user[login]</td>
        <td>$right[right]</td>
        <form name='form' action='index.php?page=admin&action=right&userID=$user[ID]' method='post'>
            <td><div class='radio'>
                <label class='radio-inline'><input type='radio' name='set' value='2'>Author</label>
                <label class='radio-inline'><input type='radio' name='set' value='3'>Reviewer</label>
                <label class='radio-inline'><input type='radio' name='set' value='0'>Admin</label>
            </div></td>
            <td><button type='submit' class='btn btn-link glyphicon glyphicon-refresh' style='color:blue'></button>
            <a href='index.php?page=admin&action=delete_user&userID=$user[ID]' class='btn btn-link glyphicon glyphicon-remove' style='color:red'></a></td>
        </form></tr>";
}
                    
?>
            
                    
            </tbody>
            </table>