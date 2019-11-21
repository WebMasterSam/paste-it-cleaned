import React from 'react'
import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl, Table, TableHead, TableRow, TableBody, TableCell, TablePagination, FormGroup, FormControlLabel } from '@material-ui/core'

import FileCopyIcon from '@material-ui/icons/FileCopy'
import HistoryIcon from '@material-ui/icons/History'
import FormWrapper from '../../../components/FormWrapper'

import './AnalyticsHits.less'
import moment from 'moment'
import { WordIcon } from './components/Word'
import { ExcelIcon } from './components/Excel'
import { PowerPointIcon } from './components/PowerPoint'
import { WebIcon } from './components/Web'
import { TextIcon } from './components/Text'
import { ImageIcon } from './components/Image'

export interface AnalyticsHitsProps {}
export interface AnalyticsHitsState {
    page: number
    rowsPerPage: number
}

const columns = [
    { id: 'timeStamp', label: 'Date/time', minWidth: 150 },
    { id: 'type', label: 'Type', minWidth: 100 },
    { id: 'ip', label: 'Client IP', minWidth: 75 },
    { id: 'userAgent', label: 'User agent', minWidth: 150 },
    { id: 'price', label: 'Price (USD)', align: 'right', minWidth: 75 },
]

function createData(timeStamp: Date, ip: string, type: string, userAgent: string, price: number) {
    var timeStampNode = moment(timeStamp).format('YYYY-MM-DD HH:MM:SS')
    var ipNode = (
        <a target="_blank" href={`https://iplogger.org/ip-lookup/?d=${ip}`}>
            {ip}
        </a>
    )
    var userAgentNode = userAgent
    var priceNode = '$' + price.toString()
    var typeNode = <span className="hit-type">?</span>

    switch (type.toLowerCase()) {
        case 'word':
            typeNode = (
                <span className="hit-type">
                    <WordIcon /> Word
                </span>
            )
            break
        case 'excel':
            typeNode = (
                <span className="hit-type">
                    <ExcelIcon /> Excel
                </span>
            )
            break
        case 'powerpoint':
            typeNode = (
                <span className="hit-type">
                    <PowerPointIcon /> PowerPoint
                </span>
            )
            break
        case 'web':
            typeNode = (
                <span className="hit-type">
                    <WebIcon /> Web
                </span>
            )
            break
        case 'text':
            typeNode = (
                <span className="hit-type">
                    <TextIcon /> Text
                </span>
            )
            break
        case 'image':
            typeNode = (
                <span className="hit-type">
                    <ImageIcon /> Image
                </span>
            )
            break
    }

    return { key: '1', timeStamp: timeStampNode, ip: ipNode, type: typeNode, userAgent: userAgentNode, price: priceNode }
}

const rows = [
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Excel', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'PowerPoint', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Web', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Text', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Image', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
]

class AnalyticsHits extends React.Component<AnalyticsHitsProps, AnalyticsHitsState> {
    constructor(props: AnalyticsHitsProps) {
        super(props)
        this.state = { page: 0, rowsPerPage: 10 }
        this.handleChangePage = this.handleChangePage.bind(this)
        this.handleChangeRowsPerPage = this.handleChangeRowsPerPage.bind(this)
    }

    handleChangePage(event: any, newPage: any) {
        this.setState({ page: newPage })
    }

    handleChangeRowsPerPage(event: any) {
        this.setState({ rowsPerPage: event.target.value })
        this.setState({ page: 0 })
    }

    render() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Last plugin uses
                </Typography>

                <FormWrapper>
                    <div className="table-wrapper">
                        <Table stickyHeader aria-label="sticky table">
                            <TableHead>
                                <TableRow>
                                    {columns.map(column => (
                                        <TableCell key={column.id} align={column.align as any} style={{ minWidth: column.minWidth }}>
                                            {column.label}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {rows.slice(this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage).map(row => {
                                    return (
                                        <TableRow hover tabIndex={-1} key={row.key}>
                                            {columns.map(column => {
                                                const value = (row as any)[column.id] as any
                                                return (
                                                    <TableCell key={column.id} align={column.align as any}>
                                                        {value}
                                                    </TableCell>
                                                )
                                            })}
                                        </TableRow>
                                    )
                                })}
                            </TableBody>
                        </Table>
                    </div>
                    <TablePagination
                        rowsPerPageOptions={[10, 25, 100]}
                        component="div"
                        count={rows.length}
                        rowsPerPage={this.state.rowsPerPage}
                        page={this.state.page}
                        backIconButtonProps={{
                            'aria-label': 'previous page',
                        }}
                        nextIconButtonProps={{
                            'aria-label': 'next page',
                        }}
                        onChangePage={this.handleChangePage}
                        onChangeRowsPerPage={this.handleChangeRowsPerPage}
                    />
                </FormWrapper>
            </Paper>
        )
    }
}

export default AnalyticsHits
