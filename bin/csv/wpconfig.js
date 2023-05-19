const path = require('path')

module.exports = {
    devtool: 'inline-source-map',
    mode: 'development',

    entry : "./src/main.ts",
    output: {
        filename: 'csv.js',
        path: path.resolve(__dirname, './BUILD'),
    },

    module: {
        rules: [
            {
                loader: 'ts-loader',
                test  : /\.ts$/,
            },
        ],
    },
    resolve: {
        extensions: [ '.ts' ],
    },
}
