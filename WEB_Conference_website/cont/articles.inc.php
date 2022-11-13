<table class="table table-hover ">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Author</th>
                        <th>Link</th>
                        <th>Score</th>
                    </tr>
                </thead>
                <tbody>
                    
<?php
                    
$db = new db_articles();
$db->Connect();
                    
$articles = $db->GetAllArticles();
                    
foreach($articles as $art)
{
    if($art["pass"] == 0) continue;
    
    $user = $db->GetUser($art["author"]);
    $score = $db->GetOverallScore($art["score1"], $art["score2"], $art["score3"]);
    
    echo "<tr><td>$art[title]</td>
        <td>$user[login]</td>
        <td><a href='pdf/$art[URL]'>$art[URL]</a></td>
        <td>$score</td></tr>";
}
                    
                    
?>
               
                </tbody>
            </table>