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

<body class="d-flex flex-column">

    @include('blog.navigation')

    <div id="content" class="container flex-grow-1">
        <div class="row mt-3">
            <main id="primary" class="col-md-8 col-xs-12 ">
                @yield('content')



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

    <footer class="bg-dark mt-5 ">
        <div class="container p-3 text-white text-center">
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
