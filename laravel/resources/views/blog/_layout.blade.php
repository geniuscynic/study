<!doctype html>
<html lang="en">

<head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="{{URL::asset('css/app.css')}}" />
    <link rel="stylesheet" href="{{URL::asset('css/blog.css')}}" />

    <title>@yield('title')</title>
</head>

<body>

    <nav id="header" class="navbar navbar-expand-lg navbar-light">
        <div class="container">
        <a class="navbar-brand" href="{{env('APP_URL')}}">
                <img alt="囧人囧事" src="{{URL::asset('img/logo.jpg')}}" />

            </a>

            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
                aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            
            <div class="collapse navbar-collapse  align-self-stretch" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto align-self-stretch">
                    <li class="nav-item active">
                        <a class="nav-link" href="#">最新</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">热门</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button"
                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Dropdown
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="dropdown-item" href="#">Action</a>
                            <a class="dropdown-item" href="#">Another action</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#">Something else here</a>
                        </div>
                    </li>

                </ul>

            </div>
        </div>
    </nav>

    <div id="content" class="container">
        <div class="row mt-3">
            <main id="primary" class="col-md-8 col-xs-12 ">
                @yield('content')


                <nav aria-label="Page navigation" class="mt-3">
                    <ul class="pagination">
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                            </a>
                        </li>
                        <li class="page-item"><a class="page-link" href="#">1</a></li>
                        <li class="page-item"><a class="page-link" href="#">2</a></li>
                        <li class="page-item"><a class="page-link" href="#">3</a></li>
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                            </a>
                        </li>
                    </ul>
                </nav>
            </main>

            <aside id="secondary" class="col-md-4">


                <!-- 边栏显示最近文章，默认注释掉 -->


                <!-- 边栏显示最近回复，默认开启 -->
                <section class="widget  mt-3">
                    <ul class="list-group">
                        <li class="list-group-item list-group-item-info"><span>最近回复</span></li>
                        <li class="list-group-item comment-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/42/comment-page-1#comment-98">dddddddddddd</a>：
                            东想西想寻寻寻寻寻寻寻寻寻寻寻寻寻寻寻寻寻寻</li>
                        <li class="list-group-item comment-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/42/comment-page-1#comment-97">看k'm</a>：
                            古巴好吧</li>
                        <li class="list-group-item comment-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/38/comment-page-1#comment-96">seerking</a>：
                            谢谢作者</li>
                    </ul>
                </section>


                <!-- 边栏显示文章分类 默认开启-->

                <!-- 边栏显示归档，默认注释掉 -->


                <!--    边栏显示其他操作，默认开启-->
                <section class="widget  mt-3">
                    <ul class="list-group">
                        <li class="list-group-item list-group-item-info"><span>其他</span></li>
                        <li class="list-group-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/admin/login.php">登录</a></li>
                        <li class="list-group-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/feed/">文章 RSS</a></li>
                        <li class="list-group-item"><a
                                href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/feed/comments/">评论 RSS</a>
                        </li>
                    </ul>
                </section>


                <!-- 边栏显示友情链接，默认注释掉 -->
                <!--    <section class="widget">-->
                <!--        <ul class="list-group">-->
                <!--            <li class="list-group-item list-group-item-info"><span>-->
                <!--</span></li>-->
                <!--            <li class="list-group-item"><a target="_blank" href="" title="">友情链接名称</a></li>-->
                <!--        </ul>-->
                <!--    </section>-->

            </aside>
        </div>
    </div>

    <footer class="bg-dark mt-5" >
      <div class="container p-3 text-white">
                Copyright 2019 , Theme Preview <span class="sep"> , </span>
                Theme by <a href="http://www.getbeststuff.com/">Discount Promo Codes</a>
</div>
      
    </footer>


    <!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="{{URL::asset('js/app.js')}}">
    </script>
    
</body>

</html>
