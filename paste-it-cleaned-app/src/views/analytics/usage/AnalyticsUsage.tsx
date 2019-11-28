import React, { Fragment } from 'react'

import { Paper, Typography, Button } from '@material-ui/core'
import FileCopyIcon from '@material-ui/icons/FileCopy'
import SkipPreviousIcon from '@material-ui/icons/SkipPrevious'
import SkipNextIcon from '@material-ui/icons/SkipNext'

import { CartesianGrid, XAxis, YAxis, Legend, Tooltip, LineChart, Line, ResponsiveContainer } from 'recharts'

import { WordIcon } from '../../../icons/Word'
import { ExcelIcon } from '../../../icons/Excel'
import { PowerPointIcon } from '../../../icons/PowerPoint'

import { WebIcon } from '../../../icons/Web'
import { TextIcon } from '../../../icons/Text'
import { ImageIcon } from '../../../icons/Image'

import './AnalyticsUsage.less'

export interface AnalyticsUsageProps {}
export interface AnalyticsUsageState {
    year: number
    month: number
}

function createDataOffice(num: number) {
    return {
        name: num.toString(),
        Word: Math.floor(Math.random() * 20),
        Excel: Math.floor(Math.random() * 20),
        PowerPoint: Math.floor(Math.random() * 20),
    }
}

function createDataWeb(num: number) {
    return {
        name: num.toString(),
        Web: Math.floor(Math.random() * 20),
    }
}

function createDataLocal(num: number) {
    return {
        name: num.toString(),
        Text: Math.floor(Math.random() * 20),
        Image: Math.floor(Math.random() * 20),
    }
}

const dataOffice = [
    createDataOffice(1),
    createDataOffice(2),
    createDataOffice(3),
    createDataOffice(4),
    createDataOffice(5),
    createDataOffice(6),
    createDataOffice(7),
    createDataOffice(8),
    createDataOffice(9),
    createDataOffice(10),
    createDataOffice(11),
    createDataOffice(12),
    createDataOffice(13),
    createDataOffice(14),
    createDataOffice(15),
    createDataOffice(16),
    createDataOffice(17),
    createDataOffice(18),
    createDataOffice(19),
    createDataOffice(20),
    createDataOffice(21),
    createDataOffice(22),
    createDataOffice(23),
    createDataOffice(24),
    createDataOffice(25),
    createDataOffice(26),
    createDataOffice(27),
    createDataOffice(28),
    createDataOffice(29),
    createDataOffice(30),
    createDataOffice(31),
]

const dataWeb = [
    createDataWeb(1),
    createDataWeb(2),
    createDataWeb(3),
    createDataWeb(4),
    createDataWeb(5),
    createDataWeb(6),
    createDataWeb(7),
    createDataWeb(8),
    createDataWeb(9),
    createDataWeb(10),
    createDataWeb(11),
    createDataWeb(12),
    createDataWeb(13),
    createDataWeb(14),
    createDataWeb(15),
    createDataWeb(16),
    createDataWeb(17),
    createDataWeb(18),
    createDataWeb(19),
    createDataWeb(20),
    createDataWeb(21),
    createDataWeb(22),
    createDataWeb(23),
    createDataWeb(24),
    createDataWeb(25),
    createDataWeb(26),
    createDataWeb(27),
    createDataWeb(28),
    createDataWeb(29),
    createDataWeb(30),
    createDataWeb(31),
]

const dataLocal = [
    createDataLocal(1),
    createDataLocal(2),
    createDataLocal(3),
    createDataLocal(4),
    createDataLocal(5),
    createDataLocal(6),
    createDataLocal(7),
    createDataLocal(8),
    createDataLocal(9),
    createDataLocal(10),
    createDataLocal(11),
    createDataLocal(12),
    createDataLocal(13),
    createDataLocal(14),
    createDataLocal(15),
    createDataLocal(16),
    createDataLocal(17),
    createDataLocal(18),
    createDataLocal(19),
    createDataLocal(20),
    createDataLocal(21),
    createDataLocal(22),
    createDataLocal(23),
    createDataLocal(24),
    createDataLocal(25),
    createDataLocal(26),
    createDataLocal(27),
    createDataLocal(28),
    createDataLocal(29),
    createDataLocal(30),
    createDataLocal(31),
]

class AnalyticsUsage extends React.Component<AnalyticsUsageProps, AnalyticsUsageState> {
    constructor(props: AnalyticsUsageState) {
        super(props)
        var curDate = new Date()
        this.state = { year: curDate.getFullYear(), month: curDate.getMonth() }
    }

    handleChange(event: React.ChangeEvent<{ name?: string; value: unknown }>, child: React.ReactNode) {}

    render() {
        return (
            <Fragment>
                {this.getFilters()}
                {this.getOfficeChart()}
                {this.getWebsiteChart()}
                {this.getLocalChart()}
            </Fragment>
        )
    }

    private getFilters() {
        return (
            <Fragment>
                <div className="filters">
                    <Button variant="outlined" startIcon={<SkipPreviousIcon />}>
                        Prev
                    </Button>
                    <Typography variant="body1" className="filter-month-label" component="span">
                        December 2018
                    </Typography>
                    <Button variant="outlined" endIcon={<SkipNextIcon />}>
                        Next
                    </Button>
                </div>
            </Fragment>
        )
    }

    private getOfficeChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Office plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={dataOffice} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Word" stroke="#1565C0" legendType="circle" activeDot={<WordDot />} />
                        <Line type="monotone" dataKey="Excel" stroke="#388E3C" legendType="circle" activeDot={<ExcelDot />} />
                        <Line type="monotone" dataKey="PowerPoint" stroke="#FF5722" legendType="circle" activeDot={<PowerPointDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getWebsiteChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Website plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={dataWeb} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Web" stroke="#d92b09" legendType="circle" activeDot={<WebDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getLocalChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Local plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={dataLocal} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Text" stroke="#333" legendType="circle" activeDot={<TextDot />} />
                        <Line type="monotone" dataKey="Image" stroke="#18a4f0" legendType="circle" activeDot={<ImageDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getMonthSubtitle() {
        return (
            <Typography variant="subtitle1" component="p" align="center">
                December 2018
            </Typography>
        )
    }

    private getLegend() {
        return <Legend />
    }

    private getCartesianGrid() {
        return <CartesianGrid strokeDasharray="3 3" />
    }

    private getXAxis() {
        return <XAxis dataKey="name" />
    }

    private getYAxis() {
        return <YAxis label={{ value: 'Pastes', angle: -90, position: 'insideLeft' }} />
    }

    private getCustomToolTip() {
        return (
            <Tooltip
                offset={30}
                contentStyle={{ borderRadius: 10 }}
                formatter={(value, name, props) => {
                    return value + ' paste' + ((value as number) > 1 ? 's' : '')
                }}
                itemStyle={{ lineHeight: 0.6 }}
                labelStyle={{ marginBottom: 10, fontWeight: 'bold' }}
                labelFormatter={(label: string | number) => {
                    var theDate = new Date(this.state.year, this.state.month, label as number)
                    return theDate.toLocaleDateString()
                }}
            />
        )
    }
}

const WordDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <WordIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const ExcelDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <ExcelIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const PowerPointDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <PowerPointIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const WebDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <WebIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const TextDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <TextIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const ImageDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <ImageIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

export default AnalyticsUsage
