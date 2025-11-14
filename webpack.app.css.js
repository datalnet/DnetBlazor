'use strict';

const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

var config = {

    optimization: {
        minimize: true,
    },

    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: "css-loader",
                        options: {
                            url: false
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            sourceMap: true,
                            implementation: require('sass'),
                            // Uncoment for not minimize
                            // sassOptions: {
                            //     outputStyle: 'expanded'
                            // }
                        },
                    },
                    {
                        loader: "postcss-loader",
                        options: {
                            postcssOptions: {
                                plugins: [
                                    [
                                        "autoprefixer",
                                    ],
                                ],
                            },
                        },
                    },
                ],
            },
        ]
    },

    plugins: [

        new MiniCssExtractPlugin({
            filename: "[name].css",
        }),
    ]
};

var siteConfig = Object.assign({}, config, {
    entry: {
        site: path.resolve(__dirname, "samples/Dnet.Shared/assets/scss/site.scss"),
    },
    output: {
        path: path.resolve(__dirname, "samples/Dnet.Shared/wwwroot/css"),
    },
});

var dnetBlazor = Object.assign({}, config, {
    entry: {
        'dnet-blazor-styles': path.resolve(__dirname, "src/Dnet.Blazor/Components/Assets/scss/dnet-blazor-styles.scss"),
        'dnet-blazor': path.resolve(__dirname, "src/Dnet.Blazor/Components/Assets/js/dnet-blazor.js"),
        // 'rxjs.min': path.resolve(__dirname, "src/Dnet.Blazor/Components/Assets/js/rxjs.min.js"),
    },
    output: {
        path: path.resolve(__dirname, "src/Dnet.Blazor/wwwroot"),
    },
    plugins: [
        ...config.plugins,
        new CopyWebpackPlugin({
            patterns: [
                {
                    from: path.resolve(__dirname, 'node_modules/@silvia-odwyer/photon/photon_rs_bg.wasm'),
                    to: path.resolve(__dirname, 'src/Dnet.Blazor/wwwroot/photon_rs_bg.wasm')
                },
                {
                    from: path.resolve(__dirname, 'node_modules/@silvia-odwyer/photon/photon_rs.js'),
                    to: path.resolve(__dirname, 'src/Dnet.Blazor/wwwroot/photon_rs.js')
                }
            ]
        })
    ]
});

var workingConfig = (env) => {
    switch (env.appname) {
        case 'site':
            return siteConfig;
        case 'dnetBlazor':
            return dnetBlazor;
    }
};

module.exports = (env) => {
    return workingConfig(env)
};